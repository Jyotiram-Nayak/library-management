﻿namespace library_management.Data.ViewModel.Email
{
    public class EmailMessage
    {
        public List<string> ToEmails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<KeyValuePair<string, string>> PlaceHolders { get; set; }
    }
}
