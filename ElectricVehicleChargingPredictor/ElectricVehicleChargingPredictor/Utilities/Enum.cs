namespace ElectricVehicleChargingPredictor
{
    public enum EmailTypeEnum //value must be same with dbo.EmailCategory
    {
        //Email Type
        ForgotPassword = 1,
        RequestComplete = 2
    }

    public enum RoleEnum
    {
        CAMSAdmin = 1,
        Internal = 2,
        Vendor = 3,
        Contractor = 4,
        External = 5
    }

    public static class AdministratorActivityEnum
    {
        public static string UserPasswordUpdate = "User Password Update";
    }

    public static class SAPArchiveEnum
    {
        public static string AlreadyUploaded = "P9";        
    }

}
