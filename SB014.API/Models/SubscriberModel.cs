using System;
using System.ComponentModel.DataAnnotations;

namespace SB014.Api.Models
{
    public class SubscriberModel
    {
        Guid id {get; set;}
        Guid TouenamnetId {get; set;}
        public string Name { get; set; }
    }
}