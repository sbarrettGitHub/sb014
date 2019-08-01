using System;
using System.ComponentModel.DataAnnotations;

namespace SB014.API.Models
{
    public class SubscriberModel
    {
        Guid id {get; set;}
        Guid TouenamnetId {get; set;}
        public string Name { get; set; }
    }
}