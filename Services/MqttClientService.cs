/**
 * (c) QA Ingenieros Ltda., Bogotá Colombia
 * 
 * 
 * Contains code authored by:    
 *   
 *   Alejandro Mejia 
 *   Santiago Urueña
 *   and others as recnognizad or listed in the code.
 */
using System;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
// using System.DrawingCore;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

// Librerias para manejo de MQTT
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using Mqtt.Client.AspNetCore.Settings;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;

using Json.Net;

// Logging
using Microsoft.Extensions.Logging;

// Librerias para manejo de tareas asincronicas
using System.Threading;
using System.Threading.Tasks;

// Librerias para manejo de Notificaciones Cliente - Servidor
using Microsoft.AspNetCore.SignalR.Client;


// Modelos para Base de datos
using AspStudio.Models;
using AspStudio.Data;

/// <summary>
/// Esta función crea las clases con los parámetros entregados por el dispositivo
/// Se crean otras subclases para clasificar dichos parámetros
/// </summary>
/// <param name="">
/// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
/// </param>

namespace Mqtt.Client.AspNetCore.Services
{
    public class MqttObj {
        public string topic {get; set;}
        public string msg {get; set;}
    }

    public class deviceObj {
        public int code {get; set;}
        public string msg {get; set;}
        public string device_id {get; set;}
        public Int64 dev_cur_pts {get; set;}
        public string tag {get; set;}
    }

    public class deviceDataObj {
        public string device_token {get; set;}
    }

    public class connectedDevices {   
        public DateTime? last_heartbeat {get; set;}     
        public string device_id {get; set;}
        public string device_name {get; set;}
    }

    public class picLibStatus {
        public string pic_url {get; set;}
        public int pic_status {get; set;}
        public string user_id {get; set;}
    }
    

    public class deviceDataBasicObj {
        public string dev_pwd {get; set;}
        public string dev_name {get; set;}
    }

    public class deviceDataNetConfObj {
        
        public string ip_addr {get; set;}
        public string net_mask {get; set;}
        public string gateway {get; set;}
        public string DDNS1 {get; set;}
        public string DDNS2 {get; set;}
        public int DHCP {get; set;}
    }

    public class deviceDataFaceRecObj {
        
        public int dec_face_num_cur {get; set;}
        public int dec_interval_cur {get; set;}
        public int dec_face_num_min {get; set;}
        public int dec_face_num_max {get; set;}
        public int dec_interval_min {get; set;}
        public int dec_interval_max {get; set;}
    }

    public class deviceDataVerInfoObj {
        
        public string dev_model {get; set;}
        public string firmware_ver {get; set;}
        public string firmware_date {get; set;}
    }

    public class deviceDataFunParObj {
        
        public bool temp_dec_en {get; set;}
        public bool stranger_pass_en {get; set;}
        public bool make_check_en {get; set;}
        public float alarm_temp {get; set;}
        public float temp_comp {get; set;}
        public int record_save_time {get; set;}

        public bool save_record {get; set;}
        public bool save_jpeg {get; set;}

    }

    public class deviceDataMqttProObj {
        
        public bool enable {get; set;}
        public bool retain {get; set;}
        public int pqos {get; set;}
        public int sqos {get; set;}
        public int port {get; set;}
        public string server {get; set;}
        public string username {get; set;}
        public string passwd {get; set;}
        public string topic2pulish {get; set;}
        public string topic2subscribe {get; set;}
        public int heartbeat {get; set;}
        

    }
    /// <summary>
    /// Instancia principal para la comunicación MQTT del backend del proyecto
    /// 
    /// </summary>
    /// <param name="">
    /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
    /// </param>
    public class MqttClientService : IMqttClientService
    {
        // Instancias para manejo de la libreria de MQTTnet
        private IMqttClient mqttClient;
        private IMqttClientOptions options;  

        private connectedDevices connectedDevices;  

        // Inyeccion clase para manejo de la conexion a BD
        private readonly IServiceScopeFactory _scopeFactory;


       
        // private readonly ApplicationDbContext dbContext;
        HubConnection connection;
        
        public MqttClientService(IMqttClientOptions options, IServiceScopeFactory scopeFactory)
        {
            this.options = options;
            mqttClient = new MqttFactory().CreateMqttClient();

            // this.conDevices = [];
            // Creamos el objeto dbContext
            // dbContext = _dbContext;
            _scopeFactory = scopeFactory;

            // Configuracion del servicio MQTTClient
            ConfigureMqttClient();

            // Configuracion del servicio de mensajeria Cliente-Servidor
            ConfigureHub();
        }
        /// <summary>
        /// Esta función realiza la configuración pára realizar la comunicación entre el backend y el frontend
        /// 
        /// </summary>
        /// <param name="">
        /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
        /// </param>

        private void ConfigureHub() 
        {
            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5050/note")
                .Build();
            // Thread.Sleep(10000);
            
        }
        /// <summary>
        /// Esta función realiza la configuración del cliente MQTT asingándolo a la misma plataforma
        /// 
        /// </summary>
        /// <param name="">
        /// La función no tiene parámetros, por lo cual realiza la configuración sin retornar nada
        /// </param>
        private void ConfigureMqttClient()
        {
            mqttClient.ConnectedHandler = this;
            mqttClient.DisconnectedHandler = this;
            mqttClient.ApplicationMessageReceivedHandler = this;
        }
        /// <summary>
        /// Esta función realiza el tratamiento del mensaje recibido por la aplicación para luego enviarla al cliente y así poder ser visualizado
        /// </summary>
        /// <param name="eventArgs">
        /// El parámetro es el mensaje recibido por la plataforma, realizar el tratamiento de dicho mensaje y ser enviado al frontend
        /// </param>

        public async Task HandleApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs eventArgs)
        {
            
            System.Console.WriteLine("Mensaje recibido");

            // De acuerdo al mensaje recibido se ejecuta alguna accion 
            
            
            // Funciones para decodificar los mensajes de llegada por MQTT y realizar las acciones adecuadas
            HandleReceivedMessagePayload(eventArgs.ApplicationMessage.ConvertPayloadToString());
            
            // Reconstruccion del String Json para envio por el Hub de mensajeria interna (Cliente-Servidor)
            // msg.Property("imageFile").Remove();
            string JsonMsg = eventArgs.ApplicationMessage.ConvertPayloadToString();
            // dynamic mes = (JObject)JsonConvert.DeserializeObject(JsonMsg);

            JObject msg = JObject.Parse(JsonMsg);
            if ( msg.SelectToken("datas") is JObject )
            {
                JObject datas = (JObject)msg["datas"];
                 // Si el mensaje contine una imagen la elimina
                var tipo = msg["msg"];

                System.Console.WriteLine(tipo);

                if (tipo.ToString() == "Upload Person Info!")
                {
                    datas.Property("imageFile").Remove();
                }

            } 
            
            string JsonSend = msg.ToString();
            string Topic = eventArgs.ApplicationMessage.Topic;

            System.Console.WriteLine("Mensaje enviado por el Hub");
            System.Console.WriteLine(JsonSend);
            //JsonMsg = JsonConvert.DeserializeObject(msg);

            // Envio por SignalR paraa comunicacion con el Cliente
            await SendByHub(Topic, JsonSend);

            // Se imprime el objeto por consola
            // System.Console.WriteLine("El mensaje recibido es: ");
            // System.Console.WriteLine(JsonMsg);

            
            
            
        
        }
        /// <summary>
        /// Esta función realiza la extracción de los parámetros recibidos enviados por el dispositivo
        /// 
        /// </summary>
        /// <param name="JsonMsg">
        /// El parámetro es el JSON enviado por el dispositivo, capturado por MQTTClient y notificado al ForeGround por el servicio SignalR.
        /// </param>
        public  void HandleReceivedMessagePayload(string JsonMsg) {
            DateTime localDate = DateTime.Now;
            try {
                // Se convierte la cadena del JsonMsg en un objeto dinamico para identificar el tipo de respuesta
                // De acuerdo al contenido se realizan acciones
                var mensaje = JsonConvert.DeserializeObject<dynamic>(JsonMsg);

                System.Console.WriteLine("Identificando respuesta:");
                // System.Console.WriteLine(mensaje);

                if (mensaje.code == -1)
                {
                    System.Console.WriteLine("Error de comando, el dispositivo entregó código de error ");
                    System.Console.WriteLine(mensaje.msg);

                }
                if (mensaje.code == 0)
                {
                    // Respuesta al evento Get All Params
                    if(mensaje.msg == "get param success")
                    {

                        // Creación del objeto device para ser ingresado a la base de datos
                        // Se hace la recolección de parámetros de la trama recibida. 
                        var device = new Device()
                        {
                            DevId = mensaje.device_id,
                            DevTag = mensaje.tag,
                            DevPwd = mensaje.datas.basic_parameters.dev_pwd,
                            DevName = mensaje.datas.basic_parameters.dev_name,
                            IpAddr = mensaje.datas.network_cofnig.ip_addr,
                            NetMsk = mensaje.datas.network_cofnig.net_mask,
                            NetGw = mensaje.datas.network_cofnig.gateway,
                            DDNS1 = mensaje.datas.network_cofnig.DDNS1,
                            DDNS2 = mensaje.datas.network_cofnig.DDNS2,
                            DHCP = mensaje.datas.network_cofnig.DHCP,
                            DevMdl = mensaje.datas.version_info.dev_model,
                            FwrVer = mensaje.datas.version_info.firmware_ver,
                            FwrDate = mensaje.datas.version_info.firmware_ver,
                            TempDecEn = mensaje.datas.fun_param.temp_dec_en,
                            StrPassEn = mensaje.datas.fun_param.stranger_pass_en,
                            MskChkEn = mensaje.datas.fun_param.make_check_en,
                            AlarmTemp = mensaje.datas.fun_param.alarm_temp,
                            TempComp = mensaje.datas.fun_param.temp_comp,
                            RcrdTimeSv = mensaje.datas.fun_param.record_save_time,
                            SvRec = mensaje.datas.fun_param.save_record,
                            SvJpg = mensaje.datas.fun_param.save_jpeg,
                            MqttEn = mensaje.datas.mqtt_protocol_set.enable,
                            MqttRet = mensaje.datas.mqtt_protocol_set.retain,
                            PQos = mensaje.datas.mqtt_protocol_set.pqos,
                            SQos = mensaje.datas.mqtt_protocol_set.sqos,
                            MqttPrt = mensaje.datas.mqtt_protocol_set.port,
                            MqttSrv = mensaje.datas.mqtt_protocol_set.server,
                            MqttUsr = mensaje.datas.mqtt_protocol_set.username,
                            MqttPwd = mensaje.datas.mqtt_protocol_set.passwd,
                            Topic2Pub = mensaje.datas.mqtt_protocol_set.topic2pulish,
                            Topic2Sub = mensaje.datas.mqtt_protocol_set.topic2subscribe,
                            HeartBt = mensaje.datas.mqtt_protocol_set.heartbeat,
                            lastReport = null

                        };

                        StoreDevice(device);
                    }

                    // Respuesta a la orden Bind

                    if (mensaje.msg == "mqtt bind ctrl success")
                    {
                        System.Console.WriteLine("Dispositivo enlazado correctamente.");

                        var device = new Device()
                        {
                            DevId = mensaje.device_id,
                            DevTag = mensaje.tag,
                            DevTkn = mensaje.datas.device_token,
                            Bound = true
                        };

                        // Update token in table Devices
                        StoreDeviceToken(device);

                    }
                    if (mensaje.msg == "mqtt unbind ctrl success")
                    {
                        System.Console.WriteLine("Dispositivo desenlazado correctamente.");
                        var device = new Device()
                        {
                            DevId = mensaje.device_id,
                            DevTag = mensaje.tag,
                            DevTkn = "",
                            Bound = false
                        };

                        // Update state in table Devices
                        StoreDeviceToken(device);
                    }
                    if (mensaje.msg == "basic param set success")
                    {
                        System.Console.WriteLine("Parámetros básicos cambiados exitosamente.");
                    }

                    if (mensaje.msg == "heartbeat")
                    {
                        System.Console.WriteLine("Heartbeat");
                        DateTime timestamp = DateTime.Now; 

                        this.StoreDeviceHeartBeat("7101016619989", timestamp);

                        System.Console.WriteLine("data Heartbeat");
                                        

                    }


                    if (mensaje.msg == "network param set success")
                    {
                        System.Console.WriteLine("Parámetros de red cambiados exitosamente.");
                    }
                    if (mensaje.msg == "The interface has changed and is no longer supported")
                    {
                        System.Console.WriteLine("Los parámetros de reconocimiento no pueden ser cambiados.");
                    }
                    if (mensaje.msg == "remote config set success")
                    {
                        System.Console.WriteLine("Parámetros remotos configurados exitosamente.");
                    }
                    if (mensaje.msg == "funtable param set success")
                    {
                        System.Console.WriteLine("Parámetros de temperatura cambiados exitosamente.");
                    }
                    if (mensaje.msg == "delete all piclib success!")
                    {
                        System.Console.WriteLine("Datos guardados del dispositivo borrados correctamente");
                    }
                    if (mensaje.msg == "download PicLib status")
                    {
                        System.Console.WriteLine("Librerías de imagenes actualizadas correctamente");

                        foreach (var item in mensaje.datas)
                        {
                            System.Console.WriteLine(item);
                            DateTime timestamp = DateTime.Now; 
                            var de = new DeviceEmployee()
                            {
                                DevId = mensaje.device_id,
                                UserId = item.user_id,
                                Status  = (item.picture_statues == 10 ) ? true : false ,
                                Created = timestamp
                            };
                            StoreDeviceEmployee(de);

                        }
                    }
                    if (mensaje.msg == "delete lib piclib success")
                    {
                        System.Console.WriteLine("Librerías de datos borrados correctamente");
                    }
                    if (mensaje.msg == "delete users piclib success")
                    {
                        System.Console.WriteLine("Usuarios borrados correctamente");
                    }

                    if (mensaje.msg == "Upload Person Info!") {
                        System.Console.WriteLine("Registro de persona en el Dispositivo");
                        if(mensaje.datas.user_id == "") {
                            mensaje.datas.user_id = 0;
                        };

                        // Convierte la imagen a jpeg y la graba en el directorio wwwroot
                        var image_url = ExportToImage(JsonMsg);
                        // System.Console.WriteLine(mensaje.datas.imageFile);
                        // Se envia a la base de datos el nombre de la imagen
                        mensaje.datas.imageFile = image_url;


                        // Se carga en el modelo Person los datos a almacenar en la DB
                        var persona = new Person()
                        {                            
                            MsgType = (Int32)mensaje.datas.msgType,
                            Similar = (float)mensaje.datas.similar,
                            UserId = (Int32)mensaje.datas.user_id,
                            Name = mensaje.datas.name,
                            RegisterTime = mensaje.datas.time,
                            Temperature = (float)mensaje.datas.temperature,
                            Matched = (Int32)mensaje.datas.matched,
                            Mask = (Int32)mensaje.datas.mask,
                            DevId = mensaje.device_id.ToString(),
                            imageUrl = image_url
                        };

                        System.Console.WriteLine("Registro en la tabla Upload_person ");
                        // Crea el registro en la base de datos local de las personas que ingresan
                        StorePerson(persona);
                        System.Console.WriteLine("Registro en la tabla Upload_person ");
                        // Envia los datos a la tabla fhir_data para ser transmitidos por WS
                        StoreFhirData(persona);

                    }

                }
                // Se imprime el objeto por consola
                System.Console.WriteLine("El mensaje recibido es: ");
                System.Console.WriteLine(mensaje);
                
            } catch (Exception e) {
                System.Console.WriteLine("Error leyendo mensaje de Mqtt : " + e.Message);
            }
            
        }

        /// <summary>
        /// Genera la insercion de datos en la BD SQL Server a partir de la clase Person
        /// </summary>
        /// <param name="persona">
        ///     Contiene los datos de la persona que fue detectada por la tableta
        ///     Ejemplo:
        ///     var persona = new Person()
        ///                {
        ///                    MsgType = "1",
        ///                    Similar = 98,
        ///                    UserId = 77,
        ///                    Name = "Santiago Urueña",
        ///                    RegisterTime = localDate,
        ///                    Temperature = 36,
        ///                    Matched = 1,
        ///                    imageUrl = "/upload/images/77.jpg"
        ///                };
        /// </param>
        public void StorePerson(Person persona) {
            
            using (var scope = _scopeFactory.CreateScope()) {

                // el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
                // scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
                // de lo contrario da un error 
                // Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
                // e instanciar un scope
                // revisar en el constructor la instanciacion de _scopeFactory
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                
                try {
                    dbContext.Persons.Add(persona);
                    dbContext.SaveChanges();
                } catch (Exception e) {
                    System.Console.WriteLine("Error Guardando Persona en la base de datos:" + e.Message + e.StackTrace);
                }
            }
            
        }


        public void StoreDeviceEmployee(DeviceEmployee de) {
            
            using (var scope = _scopeFactory.CreateScope()) {

                // el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
                // scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
                // de lo contrario da un error 
                // Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
                // e instanciar un scope
                // revisar en el constructor la instanciacion de _scopeFactory
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                
                try {
                    dbContext.DeviceEmployees.Add(de);
                    dbContext.SaveChanges();
                } catch (Exception e) {
                    System.Console.WriteLine("Error Guardando Dispositivo - Persona en la base de datos:" + e.Message + e.StackTrace);
                }
            }
            
        }

        public void StoreFhirData(Person persona) {
            
            using (var scope = _scopeFactory.CreateScope()) {

                // el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
                // scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
                // de lo contrario da un error 
                // Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
                // e instanciar un scope
                // revisar en el constructor la instanciacion de _scopeFactory
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                // var device = dbContext.Devices.FirstOrDefault(p => p.DevId == persona.DevId);
                var device_site = dbContext.DeviceSites.FirstOrDefault(p => p.DevId == persona.DevId);
                var employee = dbContext.Empleados.FirstOrDefault(p => p.Id == persona.UserId);

                string reportAlarm = Environment.GetEnvironmentVariable("ALARM_REPORT");
                string calInstrument = Environment.GetEnvironmentVariable("CALIBRATION_INSTRUMENT");
                string calMethod = Environment.GetEnvironmentVariable("CALIBRATION_METHOD");
                string calType = Environment.GetEnvironmentVariable("CALIBRATION_TYPE");
                string calValue = Environment.GetEnvironmentVariable("CALIBRATION_VALUE");
                

                var setReport = 0;
                if (reportAlarm == "True") {
                    setReport = (persona.Temperature > 37.3) ? 1 : 0;
                } else {
                    setReport = 0;
                }

                var fhir_data_table = new FhirData() {
                    TipoDoc = "CC",
                    NumDoc = persona.UserId,
                    FechaRegistro = persona.RegisterTime,
                    Temperature = persona.Temperature,
                    CiudadReg = device_site.CiudadReg,
                    SitioReg = device_site.SitioReg,
                    SitioRegId = device_site.SitioRegId.ToString(),
                    Lat = device_site.Lat,
                    Lon = device_site.Lon,
                    Nit = device_site.Nit,
                    Report = setReport,
                    IdLenel = 10101,
                    Instrumento = "Indra-FK02GYW-" + device_site.DevId,
                    TipoCal = "None",
                    TipoMed = "None",
                    ValCal = Convert.ToDouble(calValue)
                };


                try {
                    dbContext.FhirDatas.Add(fhir_data_table);
                    dbContext.SaveChanges();
                } catch (Exception e) {
                    System.Console.WriteLine("Error Guardando FhirData en la base de datos:" + e.Message + e.StackTrace);
                }
            }
            
        }

        /// <summary>
        /// el servicio de base de datos a traves de ApplicationDbContext es del tipo singleton 
        /// scoped por lo tanto se requiere crearlo antes de hacer el envio a la base de datos
        /// de lo contrario da un error 
        /// Para esto es necesario usar la clase Microsoft.Extensions.DependencyInjection
        /// e instanciar un scope revisar en el constructor la instanciacion de _scopeFactory
        /// </summary>
        /// <param name="dev"></param>
        public void StoreDevice(Device dev)
        {
            System.Console.WriteLine("Arreglo dev" + dev);
            using (var scope = _scopeFactory.CreateScope())
            {

                
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    var result = dbContext.Devices.FirstOrDefault(p => p.DevId == dev.DevId);
                    if (result != null)
                    {
                        dev.Id = result.Id;
                        dbContext.Devices.Update(dev);
                        dbContext.SaveChanges();
                    } else {
                        dbContext.Devices.Add(dev);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                }
            }

        }

        public void StoreDeviceToken(Device dev)
        {
            System.Console.WriteLine("Arreglo dev" + dev);
            using (var scope = _scopeFactory.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    var result = dbContext.Devices.FirstOrDefault(p => p.DevId == dev.DevId);
                    if (result != null)
                    {
                        if (dev.DevTkn != "") {
                            result.DevTkn = dev.DevTkn;
                        }
                        dbContext.SaveChanges();
                    } else {
                        dbContext.Devices.Add(dev);
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                }
            }

        }

        public void StoreDeviceHeartBeat(string DevId, DateTime lastReport)
        {
            System.Console.WriteLine("Registrando Heartbeat");
            
            using (var scope = _scopeFactory.CreateScope())
            {

                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    var result = dbContext.Devices.FirstOrDefault(p => p.DevId == DevId);
                    if (result != null)
                    {
                        result.lastReport = lastReport;
                        dbContext.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Error :" + e.Message + e.StackTrace);
                }
            }

        }

        /// <summary>
        /// Esta función realiza la subscripción del tópico MQTT donde se obtienen dichos parámetros
        /// 
        /// </summary>
        /// <param name="eventArgs">
        /// La función entra con el mensaje pero no retorna nada, sólo realiza la subscripción
        /// </param>
        
        public async Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("connected");
            // await mqttClient.SubscribeAsync("PublishTest");
            await mqttClient.SubscribeAsync("PublishTest");
        }

        /// <summary>
        /// Esta función se ejecuta cuando se haya perdido la conexión con el broker MQTT generando la excepción
        /// 
        /// </summary>
        /// <param name="eventArgs">
        /// La función entra con el mensaje pero no retorna nada, sólo realiza la subscripción
        /// </param>

        public async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs eventArgs)
        {
            System.Console.WriteLine("Broker Desconectado");
            var clientSettings = AppSettingsProvider.ClientSettings;
            var brokerHostSettings = AppSettingsProvider.BrokerHostSettings;
            System.Console.WriteLine("MQTT Broker :" + brokerHostSettings );
            var connected = mqttClient.IsConnected;
            var options = new MqttClientOptionsBuilder()
                .WithCredentials(clientSettings.UserName, clientSettings.Password)
                .WithClientId(clientSettings.Id)
                .WithTcpServer(brokerHostSettings.Host, brokerHostSettings.Port)
                .WithCleanSession()
                .Build();

            while (!connected )
            {
                try
                {
                    // Parametros para la configuracion del cliente MQTT.
                    // Establece conexion con el Broker
                    await mqttClient.ConnectAsync(options);
                } catch (System.Exception e) {
                    System.Console.WriteLine("No connection to MQTT Broker " + e.Message + e.StackTrace);
                }
                connected = mqttClient.IsConnected;
                await Task.Delay(30000);
            }
        }

        /// <summary>
        /// Esta función realiza la reconexión asíncrona con el broker 
        /// 
        /// </summary>
        /// <param name="cancellationToken">
        /// La función entra con el token y sólo realiza la reconexión
        /// </param>

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mqttClient.ConnectAsync(options);
            // _logger.LogWarning("Broker conectado");
            if (!mqttClient.IsConnected)
            {
                await mqttClient.ReconnectAsync();
            }
        }

        /// <summary>
        /// Esta función realiza la desconexión asíncrona con el broker 
        /// 
        /// </summary>
        /// <param name="cancellationToken">
        /// La función entra con el token y sólo realiza la desconexión
        /// </param>


        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                var disconnectOption = new MqttClientDisconnectOptions
                {
                    ReasonCode = MqttClientDisconnectReason.NormalDisconnection,
                    ReasonString = "NormalDisconnection"
                };
                await mqttClient.DisconnectAsync(disconnectOption, cancellationToken);
            }
            await mqttClient.DisconnectAsync();
        }

        /// <summary>
        ///  Convert image comming in MQTT message in format base64 to JPEG and store in Registers directory
        /// </summary>
        /// <param name="base64">
        /// base64 : String que contiene los datos de la imagen
        /// </param>
        /// <returns>
        /// imagePath : Ruta en el sistema de archivos de la imagen jpeg, para ser almacenada en la base de datos
        /// </returns>
        protected string ExportToImage(string JsonMsg)
        {
            // Convertir el string JSON a un objeto
            var mensaje = JsonConvert.DeserializeObject<dynamic>(JsonMsg);

            // extraer la informacion en formato base64 de la imagen (incluidas las cabeceras)
            string base64 = mensaje.datas.imageFile.ToString();

            // Crea un timestamp
            DateTime localDate = DateTime.Now;  

            // Separa la cabecera de los datos de la imagen
            var image64 = base64.Substring(base64.LastIndexOf(',') + 1);

            //  Convierte la cadena base64 en un arreglo de bytes
            byte[] bytes = Convert.FromBase64String(image64);

            // Obtiene la fecha del mensaje (Esto es para el mensaje MQTT)
            var fecha = mensaje.datas.time.ToString();
            fecha.Replace("/", "_");
            System.Console.WriteLine(fecha);

            // Define el nombre del archivo a guardar (Nombre de la persona + id dispositivo + fecha)
            var nombre = (mensaje.datas.name != "") ? mensaje.datas.name : "Desconocido";
            var imageName =  mensaje.device_id + '_' + nombre  + '_' + mensaje.datas.temperature  + '_' + fecha + ".jpg";
            System.Console.WriteLine(imageName);

            // Define la ruta del directorio y de la imagen
            var folderPath = "wwwroot/Registers/";
            var imagePath = folderPath + imageName;
            
            // Guarda la imagen en el sistema de archivos
            using(Image image = Image.FromStream(new MemoryStream(bytes)))
            {
                try {
                    image.Save(imagePath, ImageFormat.Jpeg);  // Or Png
                } catch(System.Exception e){
                    System.Console.WriteLine("Error saving " + imagePath + " in filesystem" + e.Message + e.StackTrace );
                }
                
            }
            
            // Devuelve el path de la imagen incluido el nombre
            return (imagePath);
        }

        /// <summary>
        /// Envio de mensajes entre el cliente y el servidor
        /// </summary>
        /// <param name="Topic"> Topico del mensaje MQTT</param>
        /// <param name="JsonSend"> Contenido</param>
        /// <returns></returns>
        public async Task SendByHub(string Topic, string JsonSend) {
            try {
                System.Console.WriteLine("Conectando al Hub");
                await connection.StartAsync();
                System.Console.WriteLine("Enviando al Hub");
                await connection.InvokeAsync("SendMessage", Topic, JsonSend);
                System.Console.WriteLine("DesConectando del Hub");
                await connection.StopAsync();
                System.Console.WriteLine("Mensaje enviado por el Hub");
            } catch (System.Exception e)
            {
                System.Console.WriteLine("Error sending Hub Message" + e.Message + e.StackTrace );
            }
        }
    }
}
