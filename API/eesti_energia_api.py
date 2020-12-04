from flask import Flask, request
from flask_restful import Resource, Api
import pandas as pd


app = Flask(__name__)
api = Api(app)

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
            
        data = data.to_json(orient='records')  # convert dataframe to json
        return data, 200  # return data and 200 OK
    
    def get_df(self):
        data = pd.read_csv('ev_models.csv')  # read local CSV
        data = data.rename(columns={'Unnamed: 0': 'model_id', 'models': 'model'})
        return data

class ChargerLocation(Resource):
    def get(self):
        data = self.get_df()
        
        data = "{d:" + data.to_json(orient='records') + "}"  # convert dataframe to json
        
        return data, 200  # return data and 200 OK
        
    def get_df(self):
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
    def get_df(self):
        file_name = "ev_long_history.csv"
        long_history_df = pd.read_csv(file_name, sep=",")
        long_history_df = long_history_df.rename(columns={'Unnamed: 0': 'id'})
        return long_history_df
    
class Vehicles(Resource):
    def get(self):
        query_parameters = request.args
        
        work_df = Works.get_df(self)
        
        model_id = query_parameters.get('model_id')
        if model_id != None:    
            work_df = work_df[work_df["model_id"] == int(model_id)]
            if len(work_df) == 0:
                return {"error_message": "Cannot find this model no"}, 404
        
        result = work_df["ev_id"].unique().to_json(orient='records') 
        #result = json.dumps(work_df["ev_id"].unique().tolist())

        return result, 200
        
class Works(Resource):
    def get_df(self):
        history_df = History.get_df(self)
        model_df = Models.get_df(self)
        location_df = ChargerLocation.get_df(self)
        
        work_df = pd.merge(left=history_df, right=model_df, how='left', left_on="model", right_on="model")
        
        work_df = pd.merge(left=work_df, right=location_df, how='left', left_on='cadaster', right_on='cadaster')
    
        return work_df
    
    
class SOC(Resource):
    def get(self):
        query_parameters = request.args
    
        ev_id = query_parameters.get('ev_id')

        work_df = Works.get_df(self);
        
        soc_predictions = work_df[(work_df["ev_id"] == int(ev_id)) & (work_df["days"] == 1)][["ev_id","time","soc"]]

        data = "{d:" + soc_predictions.to_json(orient='records') + "}"  # convert dataframe to json


        return data, 200            


api.add_resource(Models, '/Models')
api.add_resource(SOC, '/SOC')
api.add_resource(Vehicles, '/Vehicles')
api.add_resource(ChargerLocation, '/ChargerLocation')

if __name__ == '__main__':
    app.run()  # run our Flask app