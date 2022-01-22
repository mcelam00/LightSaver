namespace LightSaver.Models
{
    public class ResponseItem
    {
        //CAUTION: Exactly same names of the JSON fields in order to deserialize correctly
        public string Dia { get; set; }  //Requested date
        public string Hora { get; set; } //1-hour interval to which the following prices apply
        public decimal PCB { get; set; }    //Península, Canarias y Baleares
        public decimal CYM { get; set; }    //Ceuta y Melilla
        public decimal COF2TD { get; set; }     //---
        public decimal PMHPCB { get; set; }     //€ per MWh Península, Canarias y Baleares 
        public decimal PMHCYM { get; set; }     //€ per MWh Ceuta y Melilla
        public decimal SAHPCB { get; set; }     //Servicios de ajuste Península, Canarias y Baleares 
        public decimal SAHCYM { get; set; }     //Servicios de ajuste Ceuta y Melilla
        public decimal FOMPCB { get; set; }     //Financiación OM Península, Canarias y Baleares 
        public decimal FOMCYM { get; set; }     //Financiación OM Ceuta y Melilla
        public decimal FOSPCB { get; set; }     //Financiación OS Península, Canarias y Baleares 
        public decimal FOSCYM { get; set; }     //Financiación OS Ceuta y Melilla
        public decimal INTPCB { get; set; }     //Servicio de Ininterrumpibilidad Península, Canarias y Baleares 
        public decimal INTCYM { get; set; }     //Servicio de Ininterrumpibilidad Ceuta y Melilla
        public decimal PCAPPCB { get; set; }    //Pago por capacidad Península, Canarias y Baleares 
        public decimal PCAPCYM { get; set; }    //Pago por capacidad Ceuta y Melilla
        public decimal TEUPCB { get; set; }     //Peajes y cargos Península, Canarias y Baleares 
        public decimal TEUCYM { get; set; }     //Peajes y cargos Ceuta y Melilla
        public decimal CCVPCB { get; set; }     //Coste comercialización variable Península, Canarias y Baleares 
        public decimal CCVCYM { get; set; }     //Coste comercialización variable Ceuta y Melilla
        public decimal EDSRPCB { get; set; }    //Excedente o déficit subastas renovables Península, Canarias y Baleares 
        public decimal EDSRCYM { get; set; }    //Excedente o déficit subastas renovables Ceuta y Melilla





    }
}
