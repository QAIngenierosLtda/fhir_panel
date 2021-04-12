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
        #endregion
    }
}