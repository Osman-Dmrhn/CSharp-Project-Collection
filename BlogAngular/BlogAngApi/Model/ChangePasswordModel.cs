﻿namespace BlogAngApi.Model
{
    public class ChangePasswordModel
    {
       public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
       public  string ConfirmPassword { get; set; }
    }
}
