using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VirtuaDTO;

namespace VirtuaConecta.Wms.UI.ViewModel
{
    public class OCargaViewModel
    {
        [Display(Name = "Ordem Carga:")]
        public Int32 Id_ord_carga { get; set; }
        [Required(ErrorMessage = "Escolher transportadora")]
        [Display(Name = "Transportadora:")]
        public Int32 Id_transportadora { get; set; }
        [Required(ErrorMessage = "Escolher Data Agenda")]
        [Display(Name = "Data Agenda:")]
        public DateTime Dt_agenda{ get; set; }
        [Required(ErrorMessage = "Escolher Hora Agenda")]
        [Display(Name = "Hora Agenda:")]
        public DateTime Hora_agenda { get; set; }
        [Display(Name = "Nr coleta:")]
        public String Nr_coleta  { get; set; }
        public Int32 Nr_crt_pallet { get; set; }

        [Required(ErrorMessage = "Escolher tipo veículo")]
        [Display(Name = "Tipo veículo:")]
        public Int32 Tipo_veiculo { get; set; }
        [Required(ErrorMessage = "Escolher tipo carga")]
        [Display(Name = "Tipo carga:")]
        public Int32 Tipo_carga { get; set; }
        public String Motorista { get; set; }
        public String Placas { get; set; }
        public DateTime Dt_fim { get; set; }
        public DateTime Hora_fim { get; set; }
        public String Conferente { get; set; }
        public String Obs { get; set; }
        public String Outro_v { get; set; }

        [Required(ErrorMessage = "Escolher status")]
        [Display(Name = "Status:")]
        public Int32 Stat_evento { get; set; }
        public DateTime Data_reg { get; set; }
        public String Aberto_por { get; set; }
        public Int32 Tp { get; set; }
        public DateTime Data_coleta { get; set; }
        public Int32 Nr_pallets { get; set; }


        public SelectList Lista_transportadora { get; set; }
        public SelectList Lista_tipo_veiculo { get; set; }

        public SelectList Lista_tipo_carga { get; set; }

        public List<NfeCdocaViewModel> Lista_nfe { get; set; }

    public SelectList Lista_status_oc{ get; set; }

    }
}
