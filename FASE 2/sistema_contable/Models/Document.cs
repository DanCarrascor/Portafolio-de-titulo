using System;

namespace sistema_contable.models
{
    public class Document
    {
        public int Id { get; set; }
        public string TipoDoc { get; set; }
        public string TipoCompra { get; set; }
        public string RutProveedor { get; set; }
        public string RazonSocial { get; set; }
        public string Folio { get; set; }
        public DateTime FechaDocto { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public DateTime FechaAcuse { get; set; }
        public decimal MontoExento { get; set; }
        public decimal MontoNeto { get; set; }
        public decimal MontoIvaRecuperable { get; set; }
        public decimal MontoIvaNoRecuperable { get; set; }
        public string CodigoIvaNoRec { get; set; }
        public decimal MontoTotal { get; set; }
    }
}
