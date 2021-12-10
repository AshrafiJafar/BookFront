using System;
using System.ComponentModel.DataAnnotations;

namespace Api.MetaData
{
    public class SubTypeDtoMetaData
    {
        [Display(Name = "نسبت")]
        public Guid Id { get; set; }
    }
}
