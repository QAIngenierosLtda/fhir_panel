using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Threading.Tasks;
using DataConduitManager.Repositories.Interfaces;
using DataConduitManager.Repositories.DTO;

namespace DataConduitManager.Repositories.Logic
{
    public class Reader : IReader
    {
        private readonly IDataConduITMgr _dataConduITMgr;

        enum tipoEvento : ushort
        {
            IB, //INGRESO BIOMETRICO
            IBNI,  //INGRESO BIOMETRICO PERSONA NO IDENTIFICADA
            SB,  //SALIDA BIOMETRICO
            SBNI  //SALIDA BIOMETRICO PERSONA NO IDENTIFICADA
        }

        #region CONSTRUCTOR
        public Reader(IDataConduITMgr dataConduITMgr)
        {
            _dataConduITMgr = dataConduITMgr;
        }
        #endregion

        #region METODOS READER
        public async Task<ManagementObjectSearcher> GetReaderData(string panelID, string readerID,
            string path, string user, string pass)
        {
            ManagementScope readerScope = _dataConduITMgr.GetManagementScope(path, user, pass);
            if (readerScope == null) { throw new Exception("No fue posible establecer conexion con OnGuard"); }

            ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                "AND ReaderID='" + readerID + "'");
            ManagementObjectSearcher getReader = new ManagementObjectSearcher(readerScope, readerSearcher);

            try { return getReader; }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public async Task<int> ReaderGetMode(string panelID, string readerID, string path, string user, string pass)
        {
            try
            {
                ManagementScope readerScope = _dataConduITMgr.GetManagementScope(path, user, pass);

                ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                    "AND ReaderID='" + readerID + "'");
                ManagementObjectSearcher getreader = new ManagementObjectSearcher(readerScope, readerSearcher);

                foreach (ManagementObject queryObj in getreader.Get())
                {
                    ManagementBaseObject outParamObject = queryObj.InvokeMethod("GetMode", null, null);

                    if (outParamObject != null)
                    {
                        object outObj = outParamObject["Mode"];
                        Int32 modeReader = (Int32)outObj;
                        return modeReader;
                    }
                    else
                    {
                        throw new Exception("No se pudo consultar");
                    }
                }

                throw new Exception("No se pudo consultar");
            }
            catch (Exception ex)
            {
                throw new Exception("el dispositivo no existe " + ex.Message);
            }
        }

        public async Task<bool> ReaderSetMode(string panelID, string readerID, IReader.readerMode modeID,
            string path, string user, string pass)
        {
            try
            {
                ManagementScope readerScope = _dataConduITMgr.GetManagementScope(path, user, pass);

                ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                    "AND ReaderID='" + readerID + "'");
                ManagementObjectSearcher getreader = new ManagementObjectSearcher(readerScope, readerSearcher);

                foreach (ManagementObject queryObj in getreader.Get())
                {
                    ManagementBaseObject inParams = queryObj.GetMethodParameters("SetMode");

                    inParams.Properties["Mode"].Value = modeID;

                    queryObj.InvokeMethod("SetMode", inParams, null);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible enviar el evento " + ex.Message);
            }
        }

        public async Task<bool> OpenDoor(string panelID, string readerID, string path, string user, string pass) {
            try
            {
                ManagementScope readerScope = _dataConduITMgr.GetManagementScope(path, user, pass);

                ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                    "AND ReaderID='" + readerID + "'");
                ManagementObjectSearcher getreader = new ManagementObjectSearcher(readerScope, readerSearcher);

                foreach (ManagementObject queryObj in getreader.Get())
                {
                    //string receive = _dataConduITMgr.ReceiveEvent(readerScope);???
                    queryObj.InvokeMethod("OpenDoor", null, null);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible realizar la apertura " + ex.Message);
            }

        }

        public async Task<object> BlockDoor(string panelID, string readerID, string path, string user, string pass)
        {
            try
            {
                ManagementScope readerScope = _dataConduITMgr.GetManagementScope(path, user, pass);

                ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                    "AND ReaderID='" + readerID + "'");
                ManagementObjectSearcher getreader = new ManagementObjectSearcher(readerScope, readerSearcher);

                foreach (ManagementObject queryObj in getreader.Get())
                {
                    ManagementBaseObject inParams = queryObj.GetMethodParameters("SetMode");

                    inParams.Properties["Mode"].Value = 0;

                    queryObj.InvokeMethod("SetMode", inParams, null);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible realizar el bloqueo " + ex.Message);
            }
        }
        #endregion

        #region LOGICAL DEVICES
        public async Task<bool> SendIncomingEvent(SendEvent_DTO evento, string path, string user, string pass)
        {
            try
            {
                ManagementScope eventScope = _dataConduITMgr.GetManagementScope(path, user, pass);
                ManagementClass eventClass = new ManagementClass(eventScope, new ManagementPath("Lnl_IncomingEvent"), null);
                ManagementObject eventInstance = eventClass.CreateInstance();

                ManagementBaseObject inParams = eventClass.GetMethodParameters("SendIncomingEvent");
                
                inParams.Properties["Source"].Value = evento.source;
                
                if (!string.IsNullOrEmpty(evento.device))
                    inParams.Properties["Device"].Value = evento.device;
                
                if (!string.IsNullOrEmpty(evento.subdevice))
                    inParams.Properties["SubDevice"].Value = evento.subdevice;

                inParams.Properties["Description"].Value = evento.description;
                
                if (evento.isAccessGranted != null)
                    inParams.Properties["IsAccessGrant"].Value = evento.isAccessGranted;
                
                if (evento.isAccessDeny != null)
                    inParams.Properties["IsAccessDeny"].Value = evento.isAccessDeny;
                
                if (evento.badgeId != null)
                    inParams.Properties["BadgeID"].Value = evento.badgeId;

                // Execute the method
                eventClass.InvokeMethod("SendIncomingEvent", inParams, null);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception( ex.Message );
            }
        }

        public async Task<ManagementObjectSearcher> GetLastLocationByDoor(int panelID, int readerID, int gap, string path, string user, string pass)
        {
            try
            {
                DateTime minDate = DateTime.Now - new TimeSpan(0, 0, gap);
                DateTime maxDate = DateTime.Now + new TimeSpan(0, 0, gap);

                StringBuilder query = new StringBuilder();

                query.Append("SELECT * FROM Lnl_BadgeLastLocation WHERE PANELID = ");
                query.Append(panelID);
                query.Append(" AND READERID = ");
                query.Append(readerID);
                query.Append(" AND ACCESSFLAG = 1 AND EVENTTIME >= '");
                query.Append(minDate.ToString("yyyyMMddHHmmss"));
                query.Append(".000000-300'");
                query.Append(" AND EVENTTIME <= '");
                query.Append(maxDate.ToString("yyyyMMddHHmmss"));
                query.Append(".000000-300'");

                ManagementScope badgeScope = _dataConduITMgr.GetManagementScope(path, user, pass);
                ObjectQuery badgeSearcher = new ObjectQuery(query.ToString());

                ManagementObjectSearcher getBadge = new ManagementObjectSearcher(badgeScope, badgeSearcher);

                try { return getBadge; }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region METODOS TRANSACCIONALES
        public async Task<object> AutorizacionIngreso(SendEvent_DTO evento, int gap, int intentos, int timeOut,
            string path, string user, string password) 
        {
            
            GetCardHolderDC_DTO persona = new GetCardHolderDC_DTO();
            List<GetBadgeDC_DTO> tarjetas = new List<GetBadgeDC_DTO>();
 
            int badgekey = 0;
            string badgeID = "";

            bool salir = false;
            bool success = false;

            string resultado = "OK";
            ResLastLocationDC_DTO lastEvent = new ResLastLocationDC_DTO();
            List<LastLocation_DTO> listLocations = new List<LastLocation_DTO>();

            ManagementScope IngresoScope = _dataConduITMgr.GetManagementScope(path, user, password);

            #region OBTENER PERSONA
            if (evento.documento != null || evento.esVisitante == true)
            {
                ObjectQuery cardHolderSearcher =
                    new ObjectQuery(@"SELECT * FROM Lnl_CardHolder WHERE OPHONE = '" + evento.documento + /*"' AND SSNO = '" + ssno +*/ "'");
                ManagementObjectSearcher getCardHolder = new ManagementObjectSearcher(IngresoScope, cardHolderSearcher);

                try
                {
                    foreach (ManagementObject queryObj in getCardHolder.Get())
                    {
                        persona.id = int.Parse(queryObj["ID"].ToString());
                        try { persona.apellidos = queryObj["LASTNAME"].ToString(); } catch { persona.apellidos = null; }
                        try { persona.nombres = queryObj["FIRSTNAME"].ToString(); } catch { persona.nombres = null; }
                        try { persona.ssno = queryObj["SSNO"].ToString(); } catch { persona.ssno = null; }
                        try { persona.status = queryObj["STATE"].ToString(); } catch { persona.status = null; }
                        try { persona.documento = queryObj["OPHONE"].ToString(); } catch { persona.documento = null; }
                        try { persona.empresa = queryObj["TITLE"].ToString(); } catch { persona.empresa = null; }
                        try { persona.ciudad = queryObj["DEPT"].ToString(); } catch { persona.ciudad = null; }
                        try { persona.instalacion = queryObj["BUILDING"].ToString(); } catch { persona.instalacion = null; }
                        try { persona.piso = queryObj["FLOOR"].ToString(); } catch { persona.piso = null; }
                        try { persona.area = queryObj["DIVISION"].ToString(); } catch { persona.area = null; }
                        try { persona.email = queryObj["EMAIL"].ToString(); } catch { persona.email = null; }
                        persona.permiteVisitantes = (bool)queryObj["ALLOWEDVISITORS"];

                        #region OBTENER BADGE ACTIVO
                        ObjectQuery badgeSearcher = new ObjectQuery(@"SELECT * FROM Lnl_Badge WHERE PERSONID = '" + queryObj["ID"].ToString() + "'  AND STATUS = 1");
                        ManagementObjectSearcher getBadge = new ManagementObjectSearcher(IngresoScope, badgeSearcher);

                        foreach (ManagementObject queryObjBadge in getBadge.Get())
                        {
                            GetBadgeDC_DTO item = new GetBadgeDC_DTO();
                            item.badgeID = queryObj["ID"].ToString();
                            try
                            {
                                item.activacion = DateTime.ParseExact(queryObjBadge["ACTIVATE"].ToString().Substring(0, 14),
                                    "yyyyMMddHHmmss", null);
                            }
                            catch
                            {
                                item.activacion = null;
                            }
                            try
                            {
                                item.desactivacion = DateTime.ParseExact(queryObjBadge["DEACTIVATE"].ToString().Substring(0, 14),
                                    "yyyyMMddHHmmss", null);
                            }
                            catch
                            {
                                item.desactivacion = null;
                            }
                            try
                            {
                                item.estado = queryObjBadge["STATUS"].ToString();
                            }
                            catch
                            {
                                item.desactivacion = null;
                            }
                            try { item.type = int.Parse(queryObjBadge["TYPE"].ToString()); }
                            catch { item.type = null; }
                            try { item.badgekey = int.Parse(queryObjBadge["BADGEKEY"].ToString()); }
                            catch { item.badgekey = 0; }

                            tarjetas.Add(item);
                        }
                        #endregion

                        persona.Badges = tarjetas;
                    }

                    if (persona.id == 0)
                        evento.Desconocido = true;
                    //throw new Exception("no se encontró una persona registrada con esos datos");

                }
                catch (Exception ex)
                {
                    evento.Desconocido = true;
                    //throw new Exception(ex.Message);
                }
            }
            else
            {
                if ((bool)!evento.esVisitante)
                    evento.Desconocido = true;
            }
            #endregion

            if (persona.Badges.Count > 0)
            {
                badgekey = persona.Badges[0].badgekey;
                badgeID = persona.Badges[0].badgeID;
            }
            else { throw new Exception("la persona no tiene un badge activo"); }

            #region EVENTO
            EvaluacionEventoDC_DTO eval = new EvaluacionEventoDC_DTO();
            SendEvent_DTO acceso = new SendEvent_DTO
            {
                source = evento.source,
                device = evento.device,
                subdevice = evento.subdevice
            };
            ReaderPathDC_DTO lectora = new ReaderPathDC_DTO
            {
                panelID = evento.panelId,
                readerID = evento.readerId,
            };

            if (evento.documento != null)
                eval = GetDescripcion(tipoEvento.IB, evento);
            else
                eval = GetDescripcion(tipoEvento.IBNI, evento);

            evento.description = eval.descripcionEvento;

            //EVENTO PARA LA PGR
            bool enviado = SendIncomingEvent(IngresoScope, evento);

            //LOGICA DE ACCESO
            if (eval.alarmaEvento == false)
            {
                acceso.isAccessGranted = true;
                acceso.isAccessDeny = null;
                acceso.badgeId = int.Parse(badgeID);
                SendIncomingEvent(IngresoScope, acceso);
                OpenDoor(IngresoScope, lectora.panelID, lectora.readerID);
            }
            else
            {
                acceso.isAccessGranted = null;
                acceso.isAccessDeny = true;
                acceso.badgeId = evento.badgeId;
                SendIncomingEvent(IngresoScope, acceso);

                if ((bool)evento.validaIdentidad == false)
                {
                    #region ULTIMA UBICACION
                    try
                    {
                        int intento = 1;
                        do
                        {
                            ManagementObjectSearcher LastLocation = GetLastLocationByDoor(IngresoScope,
                                int.Parse(evento.panelId), int.Parse(evento.readerId), gap);

                            foreach (ManagementObject queryObj in LastLocation.Get())
                            {
                                LastLocation_DTO lastLocation = new LastLocation_DTO();
                                try { lastLocation.badgeId = queryObj["BADGEID"].ToString(); } catch { lastLocation.badgeId = "0"; }
                                try { lastLocation.eventTime = queryObj["EVENTTIME"].ToString(); } catch { lastLocation.eventTime = null; }
                                try { lastLocation.panelId = (int)queryObj["PANELID"]; } catch { lastLocation.panelId = 0; }
                                try { lastLocation.readerId = (int)queryObj["READERID"]; } catch { lastLocation.readerId = 0; }
                                listLocations.Add(lastLocation);
                            }

                            if (listLocations.Count > 0)
                            {
                                resultado = "OK";
                                salir = true;
                            }

                            if (intento == intentos && salir != true)
                            {
                                resultado = "No se presento un evento en la ventana de tiempo requerida";
                                salir = true;
                            }
                            else
                            {
                                System.Threading.Thread.Sleep(timeOut);
                                intento++;
                            }

                        } while (salir == false);

                        if (resultado == "OK")
                            success = true;

                        lastEvent = new ResLastLocationDC_DTO
                        {
                            locations = listLocations,
                            result = resultado,
                            success = success
                        };

                        if (lastEvent.success)
                        {

                            eval.descripcionEvento = eval.descripcionEvento + "|" + evento.documentoRfId;
                        }
                        else
                            eval.descripcionEvento = eval.descripcionEvento + "|NA";
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    #endregion
                }
            }
            if (enviado)
                return eval;
            else
                throw new Exception("No se pudo enviar el evento");

            #endregion
        }

        private EvaluacionEventoDC_DTO GetDescripcion(tipoEvento tipo, SendEvent_DTO evento)
        {
            bool eventoAlarmado = false;
            List<string> descripcion = new List<string>
            {
                tipo.ToString(), //NOMBRE DEL EVENTO
                (bool)evento.tapabocas ? "0" : "1", //ALERTA DE TAPABOCAS
                (evento.temperatura <= evento.tempRef) ? "0" : "1", //ALERTA DE TEMPERATURA
                (bool)evento.validaIdentidad? "0" : "1", //ALERTA DOCUMENTO Y BADGE NO COINCIDEN
                (bool)evento.ecoPassVencido? "0" : "1",
                (bool)evento.Desconocido? "0" : "1",
                (bool)evento.BiomeDesconectado? "0" : "1"
            };

            for (int i = 1; i < descripcion.Count; i++) 
            {
                if (descripcion[i] == "1")
                    eventoAlarmado = true;
            }

            return new EvaluacionEventoDC_DTO
            {
                descripcionEvento = descripcion[0] + "|" + descripcion[1] + "|" + descripcion[2] +
                    "|" + evento.documento.ToString() + "|" + evento.temperatura.ToString() +
                    "|" + evento.tempRef.ToString() + "|" + descripcion[3] + "|" + descripcion[4] +
                    "|" + descripcion[5] + "|" + descripcion[6],
                alarmaEvento = eventoAlarmado
            };
        }

        private bool SendIncomingEvent(ManagementScope scope, SendEvent_DTO evento) 
        {
            try
            {
                ManagementClass eventClass = new ManagementClass(scope, new ManagementPath("Lnl_IncomingEvent"), null);
                ManagementObject eventInstance = eventClass.CreateInstance();

                ManagementBaseObject inParams = eventClass.GetMethodParameters("SendIncomingEvent");

                inParams.Properties["Source"].Value = evento.source;

                if (!string.IsNullOrEmpty(evento.device))
                    inParams.Properties["Device"].Value = evento.device;

                if (!string.IsNullOrEmpty(evento.subdevice))
                    inParams.Properties["SubDevice"].Value = evento.subdevice;

                inParams.Properties["Description"].Value = evento.description;

                if (evento.isAccessGranted != null)
                    inParams.Properties["IsAccessGrant"].Value = evento.isAccessGranted;

                if (evento.isAccessDeny != null)
                    inParams.Properties["IsAccessDeny"].Value = evento.isAccessDeny;

                if (evento.badgeId != null)
                    inParams.Properties["BadgeID"].Value = evento.badgeId;

                // Execute the method
                eventClass.InvokeMethod("SendIncomingEvent", inParams, null);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool OpenDoor(ManagementScope scope, string panelID, string readerID)
        {
            try
            {
                ObjectQuery readerSearcher = new ObjectQuery("SELECT * FROM Lnl_Reader WHERE PanelID='" + panelID + "' " +
                    "AND ReaderID='" + readerID + "'");
                ManagementObjectSearcher getreader = new ManagementObjectSearcher(scope, readerSearcher);

                foreach (ManagementObject queryObj in getreader.Get())
                {
                    //string receive = _dataConduITMgr.ReceiveEvent(readerScope);???
                    queryObj.InvokeMethod("OpenDoor", null, null);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible realizar la apertura " + ex.Message);
            }

        }

        public ManagementObjectSearcher GetLastLocationByDoor(ManagementScope scope, int panelID, int readerID, int gap)
        {
            try
            {
                DateTime minDate = DateTime.Now - new TimeSpan(0, 0, gap);
                DateTime maxDate = DateTime.Now + new TimeSpan(0, 0, gap);

                StringBuilder query = new StringBuilder();

                query.Append("SELECT * FROM Lnl_BadgeLastLocation WHERE PANELID = ");
                query.Append(panelID);
                query.Append(" AND READERID = ");
                query.Append(readerID);
                query.Append(" AND ACCESSFLAG = 1 AND EVENTTIME >= '");
                query.Append(minDate.ToString("yyyyMMddHHmmss"));
                query.Append(".000000-300'");
                query.Append(" AND EVENTTIME <= '");
                query.Append(maxDate.ToString("yyyyMMddHHmmss"));
                query.Append(".000000-300'");

                ObjectQuery badgeSearcher = new ObjectQuery(query.ToString());

                ManagementObjectSearcher getBadge = new ManagementObjectSearcher(scope, badgeSearcher);

                try { return getBadge; }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}