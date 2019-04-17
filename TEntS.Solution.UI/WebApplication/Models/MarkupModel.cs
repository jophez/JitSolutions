using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication.Models
     {
     public class MarkupModel
          {
          public int Id { get; set; }
          [Required(ErrorMessage ="Code is a required field")]
          [Display(Name = "Code")]
          public string Code { get; set; }
          [Required(ErrorMessage ="Administrative is a required field")]
          [Display(Name = "Administrative")]
          [DataType(DataType.Currency, ErrorMessage = "Administrative field should be a number")]
          public float Administrative { get; set; }
          [Required(ErrorMessage = "Contingency is a required field")]
          [Display(Name = "Contingency")]
          [DataType(DataType.Currency, ErrorMessage = "Contingency field should be a number")]

          public float Contingency { get; set; }
          [Required(ErrorMessage = "Direct Labor is a required field")]
          [Display(Name = "Direct Labor")]
          [DataType(DataType.Currency, ErrorMessage = "Labor field should be a number")]

          public float DirectLabor { get; set; }
          [Required(ErrorMessage = "Equipment is a required field")]
          [Display(Name = "Equipment")]
          [DataType(DataType.Currency, ErrorMessage = "Equipment field should be a number")]

          public float Equipment { get; set; }
          [Required(ErrorMessage = "Marketing is a required field")]
          [Display(Name = "Marketing")]
          [DataType(DataType.Currency, ErrorMessage = "Marketing field should be a number")]

          public float Marketing { get; set; }
          [Required(ErrorMessage = "Markup is a required field")]
          [Display(Name = "Markup")]
          [DataType(DataType.Currency, ErrorMessage = "Markup field should be a number")]

          public float Markup { get; set; }
          [Required(ErrorMessage = "Representation is a required field")]
          [Display(Name = "Representation")]
          [DataType(DataType.Currency, ErrorMessage = "Representation field should be a number")]

          public float Representation { get; set; }
          [Required(ErrorMessage = "VAT EWT is a required field")]
          [Display(Name = "VAT EWT")]
          [DataType(DataType.Currency, ErrorMessage = "Vat field should be a number")]

          public float Vat_Ewt { get; set; }

          public float? Generic1 { get; set; }
          public float? Generic2 { get; set; }
          public float? Generic3 { get; set; }
          }
     }