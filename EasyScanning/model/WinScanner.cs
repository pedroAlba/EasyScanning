﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;


namespace EasyScanning
{
    class WinScanner
    {
        const string wiaFormatBMP = "{B96B3CAB-0728-11D3-9D7B-0000F81EF32E}";
        const string wiaFormatJPEG = "{B96B3CAE-0728-11D3-9D7B-0000F81EF32E}";


        class WIA_DPS_DOCUMENT_HANDLING_SELECT
        {
            public const uint FEEDER = 0x00000001;
            public const uint FLATBED = 0x00000002;
        }

        class WIA_DPS_DOCUMENT_HANDLING_STATUS
        {
            public const uint FEED_READY = 0x00000001;
        }

        class WIA_PROPERTIES
        {
            public const uint WIA_RESERVED_FOR_NEW_PROPS = 1024;
            public const uint WIA_DIP_FIRST = 2;
            public const uint WIA_DPA_FIRST = WIA_DIP_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPC_FIRST = WIA_DPA_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            //
            // Scanner only device properties (DPS)
            //
            public const uint WIA_DPS_FIRST = WIA_DPC_FIRST + WIA_RESERVED_FOR_NEW_PROPS;
            public const uint WIA_DPS_DOCUMENT_HANDLING_STATUS = WIA_DPS_FIRST + 13;
            public const uint WIA_DPS_DOCUMENT_HANDLING_SELECT = WIA_DPS_FIRST + 14;
        }

        /// <summary>
        /// Use scanner to scan an image (with user selecting the scanner from a dialog).
        /// </summary>
        /// <returns>Scanned images.</returns>
        public static void Scan(string tipo, string nome, String folder)
        {
            WIA.ICommonDialog dialog = new WIA.CommonDialog();
            WIA.Device device = dialog.ShowSelectDevice(WIA.WiaDeviceType.UnspecifiedDeviceType, true, false);

            if (device != null)
            {
                Scan(device.DeviceID, tipo, nome, folder);
            }
            else
            {
                throw new Exception("Você deve selecionar uma impressora para poder escanear!");
            }
        }

        /// <summary>
        /// Use scanner to scan an image (scanner is selected by its unique id).
        /// </summary>
        /// <param name="scannerName"></param>
        /// <returns>Scanned images.</returns>
        public static void Scan(string scannerId, string tipo, string nome, String folder)
        {


            bool hasMorePages = true;
            while (hasMorePages)
            {
                // select the correct scanner using the provided scannerId parameter
                WIA.DeviceManager manager = new WIA.DeviceManager();
                WIA.Device device = null;
                foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                {
                    if (info.DeviceID == scannerId)
                    {
                        // connect to scanner
                        device = info.Connect();
                        break;
                    }
                }

                // device was not found
                if (device == null)
                {
                    // enumerate available devices
                    string availableDevices = "";
                    foreach (WIA.DeviceInfo info in manager.DeviceInfos)
                    {
                        availableDevices += info.DeviceID + "n";
                    }

                    // show error with available devices
                    throw new Exception("The device with provided ID could not be found. Available Devices:n" + availableDevices);
                }

                WIA.Item item = device.Items[1] as WIA.Item;

                try
                {
                    // scan image
                    WIA.ICommonDialog wiaCommonDialog = new WIA.CommonDialog();
                    WIA.ImageFile image = (WIA.ImageFile)wiaCommonDialog.ShowTransfer(item, wiaFormatJPEG, false);
                    string finalPath = "";
                    switch (tipo)
                    {
                        case "Eletrodoméstico":
                            finalPath = folder + @"\Eletrodoméstico";
                            break;

                        case "Eletrônico":
                            finalPath = folder + @"\Eletrônico";
                            break;

                        case "Itens variados":
                            finalPath = folder + @"\Itens variados";
                            break;


                        default:
                            break;
                    }
                    nome += ".jpeg";

                    if (File.Exists(finalPath + "\\" + nome))
                    {
                        MessageBox.Show("Esse produto (" + nome + ") já está registrado!\nEscaneie novamente e informe outro nome!");
                        return;
                    }
                    else
                    {
                        image.SaveFile(finalPath + "\\" + nome);
                    }
                    hasMorePages = false;
                }
                catch (Exception exc)
                {
                    throw exc;
                }

            }
            DialogResult dialogResult = MessageBox.Show("Deseja escanear outro arquivo?", "", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                return;
            }
            else if (dialogResult == DialogResult.No)
            {
                Process.Start(folder);
                Application.Exit();
            }

        }



        /// <summary>
        /// Gets the list of available WIA devices.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetDevices()
        {
            List<string> devices = new List<string>();
            WIA.DeviceManager manager = new WIA.DeviceManager();

            foreach (WIA.DeviceInfo info in manager.DeviceInfos)
            {
                devices.Add(info.DeviceID);
            }

            return devices;
        }
    }
}

