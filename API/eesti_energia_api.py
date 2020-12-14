from flask import Flask, request
from flask_restful import Resource, Api
import pandas as pd
import json
import numpy as np
from tensorflow.keras.models import model_from_json
import tensorflow as tf
from sklearn import preprocessing
import pickle

sc = preprocessing.MinMaxScaler()
app = Flask(__name__)
api = Api(app)

class CommonFunction():
    def make_dataset(total_window_size, data):
        data = np.array(data, dtype=np.float32)
        ds = tf.keras.preprocessing.timeseries_dataset_from_array(
            data=data,
            targets=None,
            sequence_length=total_window_size,
            sequence_stride=1,
            shuffle=False,
            batch_size=32,)
        return ds
    
    def make_data_distance(ev_id, time, train_work_df):
        #example time = 15
        data = []
        counter = time

        for i in range(24):
            if(counter > 24):
                counter = 0
                
            helper = train_work_df[(train_work_df["ev_id"] == int(ev_id)) & 
                                   (train_work_df["days"] == 24) & 
                                   (train_work_df["time"] == counter)]["distance_traveled"]
            print(counter)
            data.append([int(ev_id), counter, helper.values[0]])
            counter+=1

        ds = pd.DataFrame(data, columns = ['ev_id', 'time','distance_traveled'])
    
        test_data = ds
        test_data = test_data.iloc[:, 1].values
     
        unscaled_training_data = train_work_df[['ev_id', 'time', 'distance_traveled']]
        unscaled_test_data = ds
     
        all_data = pd.concat((unscaled_training_data['distance_traveled'], unscaled_test_data['distance_traveled']), axis = 0)
        x_test_data = all_data[len(all_data) - len(test_data) - 40:].values
        x_test_data = np.reshape(x_test_data, (-1, 1))
        x_test_data = sc.fit_transform(x_test_data)
     
        final_x_test_data = []
     
        for i in range(40, len(x_test_data)):
            final_x_test_data.append(x_test_data[i-1:i, 0])
     
        final_x_test_data = np.array(final_x_test_data)
     
        final_x_test_data = np.reshape(final_x_test_data, 
                                       (final_x_test_data.shape[0],       
                                        final_x_test_data.shape[1], 1))
    
        return final_x_test_data
    
    def make_data_grid_connection(ev_id, soc, distance_traveled):
        
        data = []
        for i in range(24):
            data.append([int(ev_id), i, float(soc[i]), float(distance_traveled[i])])

        ds = pd.DataFrame(data, columns = ['ev_id', 'time', 'soc' ,'distance_traveled'])
        
        return ds
    
    def load_model(json_file_name, weight_file_name):
        # load json and create model
        json_file = open(json_file_name, 'r')
        loaded_model_json = json_file.read()
        json_file.close()
        loaded_model = model_from_json(loaded_model_json)
        # load weights into new model
        loaded_model.load_weights(weight_file_name)

        return loaded_model
    
    def load_model_pickle(filename):
        loaded_model = pickle.load(open(filename, 'rb'))
        
        return loaded_model
    
class Models(Resource):
    def __init__(self):        
        self.model_id = 0
        self.model = ""
        self.battery_size = 0
        self.charge_power = 0
        self.efficiency = 0
    
    def get(self):
        data = self.get_df(self)
        query_parameters = request.args
        
        id = query_parameters.get('id')
        if id != None:    
            data = data[data["model_id"] == int(id)]
            if len(data) == 0:
                return {"error_message": "Cannot find this model no"}, 404
            
            
        data = "{d:" + data.to_json(orient='records') + "}"  # convert dataframe
        return data, 200  # return data and 200 OK
    
    def get_df(self, args):
        data = pd.read_csv('ev_models.csv')  # read local CSV
        data = data.rename(columns={'Unnamed: 0': 'model_id', 'models': 'model'})
        return data

class ChargerLocation(Resource):
    def get(self):
        data = self.get_df()
        
        data = "{d:" + data.to_json(orient='records') + "}"  # convert dataframe to json
        
        return data, 200  # return data and 200 OK
        
    def get_df(self, args):
        file_name = "ev_home_locations.csv"
        home_location_df = pd.read_csv(file_name, sep=",")
        home_location_df = home_location_df.rename(columns={'Unnamed: 0': 'id'})
        
        file_name = "public_chargers_locations.csv"
        public_charger_location_df = pd.read_csv(file_name, sep=",")
        public_charger_location_df = public_charger_location_df.rename(columns={'Unnamed: 0': 'id'})

        home_location_df["location_type"] = "home"
        public_charger_location_df["location_type"] = "public"

        location_df = pd.concat([home_location_df, public_charger_location_df])
        location_df = location_df.rename(columns={'id': 'loc_id'})
        
        return location_df

class History(Resource):
    def get_df(self, args):
        file_name = "ev_long_history.csv"
        long_history_df = pd.read_csv(file_name, sep=",")
        long_history_df = long_history_df.rename(columns={'Unnamed: 0': 'id'})
        return long_history_df
    
class Vehicles(Resource):
    def get(self):
        query_parameters = request.args

        work_df = Works.get_df(self, query_parameters)

        model_id = query_parameters.get('model_id')
        if model_id != None:    
            work_df = work_df[work_df["model_id"] == int(model_id)]
            if len(work_df) == 0:
                return {"error_message": "Cannot find this model no"}, 404
        
        #result = work_df["ev_id"].unique();
        result = json.dumps(work_df["ev_id"].unique().tolist())

        return result, 200
        
class Works(Resource):
    def get_df(self, args):
        history_df = History.get_df(self, args)
        model_df = Models.get_df(self, args)
        location_df = ChargerLocation.get_df(self, args)
        
        work_df = pd.merge(left=history_df, right=model_df, how='left', left_on="model", right_on="model")
        
        work_df = pd.merge(left=work_df, right=location_df, how='left', left_on='cadaster', right_on='cadaster')
        work_df["connected_val"] = np.where(work_df["connected"], 1, 0) 
                
        return work_df
    
    def get_clean_df(self, args):
        
        work_df = Works.get_df(self, args)
        model_df = Models.get_df(self, args)
        
        clean_work_df = work_df[["ev_id", "model_id", "days","time", "connected", "soc", "loc_id", "location_type"]]
        clean_work_df["location_val"] = np.where(clean_work_df["location_type"] == "home", 0, 1)
        
        clean_work_df["soc_diff_charged"] = np.where(clean_work_df["soc"].diff() > 0, clean_work_df["soc"].diff(), 0)
        clean_work_df["soc_diff_used"] = np.where(clean_work_df["soc"].diff() < 0, clean_work_df["soc"].diff(), 0)

        #set days 1 = 0, because we don't want to calculate from different model
        clean_work_df[(clean_work_df["days"] == 1) & (clean_work_df["time"] == 1)]["soc_diff_charged"] = 0
        clean_work_df[(clean_work_df["days"] == 1) & (clean_work_df["time"] == 1)]["soc_diff_used"] = 0
        
        merged_inner = pd.merge(left=clean_work_df, right=model_df, left_on='model_id', right_on='model_id')
        merged_inner["soc_diff"] = abs(merged_inner["soc_diff_charged"] +  merged_inner["soc_diff_used"])
        merged_inner["distance_traveled"] = abs(merged_inner["soc_diff_used"] / merged_inner["efficiency"])
        merged_inner["connected_val"] = np.where(merged_inner["connected"], 1, 0) 
        merged_inner["location_val"] = np.where(merged_inner["location_type"] == "home", 0, 1)
        
        return merged_inner
    
    def get_train_test_df(self, args):
        
        merged_inner = Works.get_clean_df(self, args);
        tmp_df = merged_inner[merged_inner["ev_id"] == 0];
        train_work_df = tmp_df[: int(np.round(len(tmp_df) * 0.80))];
        val_work_df = tmp_df[int(np.round(len(tmp_df) * 0.80)) :];

        for i in range(1, 100):
            tmp_df = merged_inner[merged_inner["ev_id"] == i]

            train_work_df = pd.concat([train_work_df, tmp_df[: int(np.round(len(tmp_df) * 0.80))]])
            val_work_df = pd.concat([val_work_df, tmp_df[int(np.round(len(tmp_df) * 0.80)) :]])
    
        return train_work_df, val_work_df    
    
class SOC(Resource):
    def get(self):
        query_parameters = request.args
    
        ev_id = query_parameters.get('ev_id')

        work_df = Works.get_df(self, query_parameters);
    
        soc_history = work_df[(work_df["ev_id"] == int(ev_id)) ][["ev_id", "model_id","time", "connected_val","soc"]]

        loaded_model = CommonFunction.load_model("soc_predictor_model.json", "soc_predictor_model.h5")
        
        ds = CommonFunction.make_dataset(24, soc_history);

        res = loaded_model.predict(ds);

        idx = res.shape[0]-2;

        #data = "{d:" + soc_predictions.to_json(orient='records') + "}"  # convert dataframe to json


        return json.dumps(res[idx].flatten().tolist()), 200            

class DrivingBehaviour(Resource):
    def get(self):
        query_parameters = request.args

        ev_id = query_parameters.get('ev_id');

        train_work_df, val_work_df = Works.get_train_test_df(self, query_parameters);
    
        loaded_model = CommonFunction.load_model("rnn_distance_model_2.json", "rnn_distance_model_2.h5")
        
        ds = CommonFunction.make_data_distance(ev_id, 1, train_work_df);
        predictions = loaded_model.predict(ds);

        nsamples, nx, ny = predictions.shape
        d2 = predictions.reshape((nsamples,nx*ny))
        res = sc.inverse_transform(d2)

        #data = "{d:" + soc_predictions.to_json(orient='records') + "}"  # convert dataframe to json


        return json.dumps(res.flatten().tolist()), 200  

class GridConnection(Resource):
    def get(self):
        query_parameters = request.args
    
        ev_id = query_parameters.get('ev_id')

        # Predict SOC
        work_df = Works.get_df(self, query_parameters);
        soc_history = work_df[(work_df["ev_id"] == int(ev_id)) ][["ev_id", "model_id","time", "connected_val","soc"]]
        loaded_model = CommonFunction.load_model("soc_predictor_model.json", "soc_predictor_model.h5")        
        ds = CommonFunction.make_dataset(24, soc_history);
        res = loaded_model.predict(ds);
        idx = res.shape[0]-2;
        soc = res[idx]

        # Predict Distance Traveled
        train_work_df, val_work_df = Works.get_train_test_df(self, query_parameters);
        loaded_model = CommonFunction.load_model("rnn_distance_model_2.json", "rnn_distance_model_2.h5")
        ds = CommonFunction.make_data_distance(ev_id, 1, train_work_df);
        predictions = loaded_model.predict(ds);
        nsamples, nx, ny = predictions.shape
        d2 = predictions.reshape((nsamples,nx*ny))
        res = sc.inverse_transform(d2)

        distance_traveled = res.flatten().tolist()

        
        ds = CommonFunction.make_data_grid_connection(ev_id, soc, distance_traveled);
        
        loaded_model = CommonFunction.load_model_pickle("voting_model2.sav");
        
        res = loaded_model.predict(ds[['ev_id', 'time', 'soc', 'distance_traveled']])
        
        #data = "{d:" + soc_predictions.to_json(orient='records') + "}"  # convert dataframe to json


        return json.dumps(res.tolist()), 200    

api.add_resource(Models, '/Models')
api.add_resource(SOC, '/SOC')
api.add_resource(Vehicles, '/Vehicles')
api.add_resource(ChargerLocation, '/ChargerLocation')
api.add_resource(GridConnection, '/GridConnection')
api.add_resource(DrivingBehaviour, '/DrivingBehaviour')

if __name__ == '__main__':
    app.run()  # run our Flask app