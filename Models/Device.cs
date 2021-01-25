using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspStudio.Models
{
    
    // Definicion del Objeto Devices (tabla para almacenar la informacion de configuracion de los dispositivos)
    [Table("devices")]
    public class Device
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id { get; set; }

        [Column("device_id")]
        public string DevId { get; set; }

        [Column("device_tag")]
        public string DevTag { get; set; }

        [Column("device_tkn")]
        public string DevTkn { get; set; }

        [Column("device_pwd")]
        public string DevPwd { get; set; }

        [Column("device_name")]
        public string DevName { get; set; }

        [Column("ip_addr")]
        public string IpAddr { get; set; }

        [Column("net_mask")]
        public string NetMsk { get; set; }

        [Column("gateway")]
        public string NetGw { get; set; }

        [Column("DDNS1")]
        public string DDNS1 { get; set; }

        [Column("DDNS2")]
        public string DDNS2 { get; set; }

        [Column("DHCP")]
        public bool DHCP { get; set; }
        

        [Column("dev_model")]
        public string DevMdl { get; set; }

        [Column("firmware_ver")]
        public string FwrVer { get; set; }

        [Column("firmware_date")]
        public string FwrDate { get; set; }

        [Column("temp_dec_en")]
        public bool TempDecEn { get; set; }

        [Column("stranger_pass_en")]
        public bool StrPassEn { get; set; }

        [Column("mask_check_en")]
        public bool MskChkEn { get; set; }

        [Column("alarm_temp")]
        public double AlarmTemp { get; set; }

        [Column("temp_comp")]
        public double TempComp { get; set; }

        [Column("record_time_save")]
        public int RcrdTimeSv { get; set; }

        [Column("save_record")]
        public bool SvRec { get; set; }

        [Column("save_jpeg")]
        public bool SvJpg { get; set; }

        [Column("mqtt_enable")]
        public int MqttEn { get; set; }

        [Column("mqtt_retain")]
        public int MqttRet { get; set; }

        [Column("pqos")]
        public int PQos { get; set; }

        [Column("sqos")]
        public int SQos { get; set; }

        [Column("mqtt_port")]
        public int MqttPrt { get; set; }

        [Column("mqtt_server")]
        public string MqttSrv { get; set; }

        [Column("mqtt_username")]
        public string MqttUsr { get; set; }

        [Column("mqtt_password")]
        public string MqttPwd { get; set; }

        [Column("topic2publish")]
        public string Topic2Pub { get; set; }

        [Column("topic2subscribe")]
        public string Topic2Sub { get; set; }

        [Column("heartbeat")]
        public int HeartBt { get; set; }

        [Column("bound")]
        public bool Bound { get; set; }

        [Column("last_report")]
        public DateTime? lastReport {get; set;}
        
    }
}
