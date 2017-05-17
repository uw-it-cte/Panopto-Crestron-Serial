using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_UW_PANOPTO_SERIAL_PROCESSOR_V2_0
{
    public class UserModuleClass_UW_PANOPTO_SERIAL_PROCESSOR_V2_0 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput GET_STATUS;
        Crestron.Logos.SplusObjects.DigitalInput CMD_START;
        Crestron.Logos.SplusObjects.DigitalInput CMD_STOP;
        Crestron.Logos.SplusObjects.DigitalInput CMD_PAUSE;
        Crestron.Logos.SplusObjects.DigitalInput CMD_RESUME;
        Crestron.Logos.SplusObjects.DigitalInput CMD_EXTEND;
        Crestron.Logos.SplusObjects.DigitalInput ERROR_NOCOM;
        Crestron.Logos.SplusObjects.DigitalOutput RECORDING_ABOUTTOSTART;
        Crestron.Logos.SplusObjects.BufferInput PANOPTO_SERIAL_RX;
        Crestron.Logos.SplusObjects.AnalogInput ENABLE_ADHOC_START;
        Crestron.Logos.SplusObjects.AnalogInput DEBUG;
        Crestron.Logos.SplusObjects.DigitalOutput SHOW_CMD_START;
        Crestron.Logos.SplusObjects.DigitalOutput SHOW_CMD_STOP;
        Crestron.Logos.SplusObjects.DigitalOutput SHOW_CMD_PAUSE;
        Crestron.Logos.SplusObjects.DigitalOutput SHOW_CMD_RESUME;
        Crestron.Logos.SplusObjects.DigitalOutput SHOW_CMD_EXTEND;
        Crestron.Logos.SplusObjects.DigitalOutput HAS_RECORDING_DATA;
        Crestron.Logos.SplusObjects.DigitalOutput HAS_QUEUED_DATA;
        Crestron.Logos.SplusObjects.StringOutput PANOPTO_SERIAL_TX;
        Crestron.Logos.SplusObjects.StringOutput RECORDERSTATE;
        Crestron.Logos.SplusObjects.StringOutput RECORDERSTATESIMPLE;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_ID;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_NAME;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_STARTTIME;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_ENDTIME;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_MINUTESUNTILSTARTTIME;
        Crestron.Logos.SplusObjects.StringOutput RECORDING_MINUTESUNTILENDTIME;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_ID;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_NAME;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_STARTTIME;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_ENDTIME;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_MINUTESUNTILSTARTTIME;
        Crestron.Logos.SplusObjects.StringOutput QUEUED_RECORDING_MINUTESUNTILENDTIME;
        ushort GI_ALLOWCMD = 0;
        ushort GI_STATUS_COUNT_REC = 0;
        ushort GI_STATUS_COUNT_QUE = 0;
        CrestronString GS_TEMP_PANOPTO_SERIAL_RX;
        CrestronString [] GS_RECORDING_TAG;
        CrestronString GS_RECORDING_ID;
        CrestronString GS_RECORDING_NAME;
        CrestronString GS_RECORDING_STARTTIME;
        CrestronString GS_RECORDING_ENDTIME;
        CrestronString GS_RECORDING_MINUTESUNTILSTARTTIME;
        CrestronString GS_RECORDING_MINUTESUNTILENDTIME;
        CrestronString GS_QUEUED_RECORDING_ID;
        CrestronString GS_QUEUED_RECORDING_NAME;
        CrestronString GS_QUEUED_RECORDING_STARTTIME;
        CrestronString GS_QUEUED_RECORDING_ENDTIME;
        CrestronString GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME;
        CrestronString GS_QUEUED_RECORDING_MINUTESUNTILENDTIME;
        private CrestronString DEBUGPRINTHEX (  SplusExecutionContext __context__, CrestronString SDEBUG ) 
            { 
            CrestronString STEMP;
            CrestronString SDEBUGHEX;
            STEMP  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
            SDEBUGHEX  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
            
            ushort NEXTCHAR = 0;
            
            
            __context__.SourceCodeLine = 64;
            STEMP  .UpdateValue ( SDEBUG  ) ; 
            __context__.SourceCodeLine = 65;
            do 
                { 
                __context__.SourceCodeLine = 66;
                NEXTCHAR = (ushort) ( Functions.GetC( STEMP ) ) ; 
                __context__.SourceCodeLine = 67;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NEXTCHAR == 10))  ) ) 
                    { 
                    __context__.SourceCodeLine = 67;
                    SDEBUGHEX  .UpdateValue ( SDEBUGHEX + "\\x0A"  ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 68;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NEXTCHAR == 13))  ) ) 
                        { 
                        __context__.SourceCodeLine = 68;
                        SDEBUGHEX  .UpdateValue ( SDEBUGHEX + "\\x0D"  ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 69;
                        SDEBUGHEX  .UpdateValue ( SDEBUGHEX + Functions.Chr (  (int) ( NEXTCHAR ) )  ) ; 
                        } 
                    
                    }
                
                } 
            while (false == ( Functions.TestForTrue  (  (int) ( Functions.BoolToInt (Functions.Length( STEMP ) == 0) ) ) )); 
            __context__.SourceCodeLine = 71;
            return ( SDEBUGHEX ) ; 
            
            }
            
        private short FUN_ATOSI (  SplusExecutionContext __context__, CrestronString VALSTRING ) 
            { 
            short VALINT = 0;
            
            
            __context__.SourceCodeLine = 77;
            VALINT = (short) ( Functions.Atoi( VALSTRING ) ) ; 
            __context__.SourceCodeLine = 78;
            if ( Functions.TestForTrue  ( ( Functions.Find( "-" , VALSTRING ))  ) ) 
                { 
                __context__.SourceCodeLine = 78;
                VALINT = (short) ( (VALINT * Functions.ToSignedLongInteger( -( 1 ) )) ) ; 
                } 
            
            __context__.SourceCodeLine = 79;
            return (short)( VALINT) ; 
            
            }
            
        private void CLEARRECORDINGDATA (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 84;
            GS_RECORDING_ID  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 85;
            GS_RECORDING_NAME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 86;
            GS_RECORDING_STARTTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 87;
            GS_RECORDING_ENDTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 88;
            GS_RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 89;
            GS_RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( ""  ) ; 
            
            }
            
        private void CLEARQUERECORDINGDATA (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 93;
            GS_QUEUED_RECORDING_ID  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 94;
            GS_QUEUED_RECORDING_NAME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 95;
            GS_QUEUED_RECORDING_STARTTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 96;
            GS_QUEUED_RECORDING_ENDTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 97;
            GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 98;
            GS_QUEUED_RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( ""  ) ; 
            
            }
            
        private void SHOWCOMMANDS (  SplusExecutionContext __context__ ) 
            { 
            short RECTOEND = 0;
            short QUETOSTART = 0;
            
            
            __context__.SourceCodeLine = 107;
            RECTOEND = (short) ( FUN_ATOSI( __context__ , GS_RECORDING_MINUTESUNTILENDTIME ) ) ; 
            __context__.SourceCodeLine = 108;
            QUETOSTART = (short) ( FUN_ATOSI( __context__ , GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME ) ) ; 
            __context__.SourceCodeLine = 110;
            if ( Functions.TestForTrue  ( ( GI_ALLOWCMD)  ) ) 
                { 
                __context__.SourceCodeLine = 113;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "PreviewingWithNextSchedule") ) && Functions.TestForTrue ( QUETOSTART )) ) ) && Functions.TestForTrue ( Functions.BoolToInt ( QUETOSTART < 6 ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 114;
                    SHOW_CMD_START  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 116;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "PreviewingNoNextSchedule") ) && Functions.TestForTrue ( ENABLE_ADHOC_START  .UshortValue )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 117;
                        SHOW_CMD_START  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 119;
                        SHOW_CMD_START  .Value = (ushort) ( 0 ) ; 
                        } 
                    
                    }
                
                __context__.SourceCodeLine = 122;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Recording") ) || Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Paused") )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 123;
                    SHOW_CMD_STOP  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 125;
                    SHOW_CMD_STOP  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 128;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Recording"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 129;
                    SHOW_CMD_PAUSE  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 131;
                    SHOW_CMD_PAUSE  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 134;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Paused"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 135;
                    SHOW_CMD_RESUME  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 137;
                    SHOW_CMD_RESUME  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 140;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Recording") ) || Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE == "Paused") )) ) ) && Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( RECTOEND ) && Functions.TestForTrue ( QUETOSTART )) ) ) && Functions.TestForTrue ( Functions.BoolToInt ( (RECTOEND + 7) < QUETOSTART ) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt ( RECTOEND < 20 ) )) ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 141;
                    SHOW_CMD_EXTEND  .Value = (ushort) ( 1 ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 143;
                    SHOW_CMD_EXTEND  .Value = (ushort) ( 0 ) ; 
                    } 
                
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 147;
                SHOW_CMD_START  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 148;
                SHOW_CMD_STOP  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 149;
                SHOW_CMD_PAUSE  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 150;
                SHOW_CMD_RESUME  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 151;
                SHOW_CMD_EXTEND  .Value = (ushort) ( 0 ) ; 
                } 
            
            
            }
            
        private void UPDATERECORDINGDATA (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 164;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (GS_RECORDING_ID == GS_QUEUED_RECORDING_ID))  ) ) 
                { 
                __context__.SourceCodeLine = 165;
                CLEARQUERECORDINGDATA (  __context__  ) ; 
                } 
            
            __context__.SourceCodeLine = 169;
            if ( Functions.TestForTrue  ( ( Functions.Not( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Recording") ) || Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Paused") )) ) ))  ) ) 
                { 
                __context__.SourceCodeLine = 170;
                CLEARRECORDINGDATA (  __context__  ) ; 
                } 
            
            __context__.SourceCodeLine = 173;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( GI_STATUS_COUNT_REC > 5 ))  ) ) 
                { 
                __context__.SourceCodeLine = 174;
                CLEARRECORDINGDATA (  __context__  ) ; 
                __context__.SourceCodeLine = 175;
                GI_STATUS_COUNT_REC = (ushort) ( 1 ) ; 
                } 
            
            __context__.SourceCodeLine = 178;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( GI_STATUS_COUNT_QUE > 5 ))  ) ) 
                { 
                __context__.SourceCodeLine = 179;
                CLEARQUERECORDINGDATA (  __context__  ) ; 
                __context__.SourceCodeLine = 180;
                GI_STATUS_COUNT_QUE = (ushort) ( 1 ) ; 
                } 
            
            __context__.SourceCodeLine = 184;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME == "2") ) || Functions.TestForTrue ( Functions.BoolToInt (GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME == "1") )) ) ) || Functions.TestForTrue ( Functions.BoolToInt (GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME == "0") )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 185;
                RECORDING_ABOUTTOSTART  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 187;
                RECORDING_ABOUTTOSTART  .Value = (ushort) ( 0 ) ; 
                } 
            
            __context__.SourceCodeLine = 190;
            RECORDING_ID  .UpdateValue ( GS_RECORDING_ID  ) ; 
            __context__.SourceCodeLine = 191;
            RECORDING_NAME  .UpdateValue ( GS_RECORDING_NAME  ) ; 
            __context__.SourceCodeLine = 192;
            RECORDING_STARTTIME  .UpdateValue ( GS_RECORDING_STARTTIME  ) ; 
            __context__.SourceCodeLine = 193;
            RECORDING_ENDTIME  .UpdateValue ( GS_RECORDING_ENDTIME  ) ; 
            __context__.SourceCodeLine = 194;
            RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( GS_RECORDING_MINUTESUNTILSTARTTIME  ) ; 
            __context__.SourceCodeLine = 195;
            RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( GS_RECORDING_MINUTESUNTILENDTIME  ) ; 
            __context__.SourceCodeLine = 197;
            QUEUED_RECORDING_ID  .UpdateValue ( GS_QUEUED_RECORDING_ID  ) ; 
            __context__.SourceCodeLine = 198;
            QUEUED_RECORDING_NAME  .UpdateValue ( GS_QUEUED_RECORDING_NAME  ) ; 
            __context__.SourceCodeLine = 199;
            QUEUED_RECORDING_STARTTIME  .UpdateValue ( GS_QUEUED_RECORDING_STARTTIME  ) ; 
            __context__.SourceCodeLine = 200;
            QUEUED_RECORDING_ENDTIME  .UpdateValue ( GS_QUEUED_RECORDING_ENDTIME  ) ; 
            __context__.SourceCodeLine = 201;
            QUEUED_RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME  ) ; 
            __context__.SourceCodeLine = 202;
            QUEUED_RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( GS_QUEUED_RECORDING_MINUTESUNTILENDTIME  ) ; 
            __context__.SourceCodeLine = 206;
            if ( Functions.TestForTrue  ( ( Functions.Length( GS_RECORDING_ID ))  ) ) 
                { 
                __context__.SourceCodeLine = 206;
                HAS_RECORDING_DATA  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 206;
                HAS_RECORDING_DATA  .Value = (ushort) ( 0 ) ; 
                } 
            
            __context__.SourceCodeLine = 207;
            if ( Functions.TestForTrue  ( ( Functions.Length( GS_QUEUED_RECORDING_ID ))  ) ) 
                { 
                __context__.SourceCodeLine = 207;
                HAS_QUEUED_DATA  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 207;
                HAS_QUEUED_DATA  .Value = (ushort) ( 0 ) ; 
                } 
            
            __context__.SourceCodeLine = 210;
            SHOWCOMMANDS (  __context__  ) ; 
            
            }
            
        private void SENDCOMMAND (  SplusExecutionContext __context__, CrestronString SCMD ) 
            { 
            ushort NOWTIME = 0;
            ushort NEWTIME = 0;
            ushort OLDTIME = 0;
            ushort DIFFTIME = 0;
            
            
            __context__.SourceCodeLine = 219;
            PANOPTO_SERIAL_TX  .UpdateValue ( SCMD + "\r\n"  ) ; 
            __context__.SourceCodeLine = 220;
            GI_ALLOWCMD = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 221;
            SHOWCOMMANDS (  __context__  ) ; 
            __context__.SourceCodeLine = 224;
            NOWTIME = (ushort) ( Functions.GetHSeconds() ) ; 
            __context__.SourceCodeLine = 225;
            NEWTIME = (ushort) ( (NOWTIME + (30 * 100)) ) ; 
            __context__.SourceCodeLine = 227;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NEWTIME > NOWTIME ))  ) ) 
                { 
                __context__.SourceCodeLine = 228;
                while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (GI_ALLOWCMD == 0) ) && Functions.TestForTrue ( Functions.BoolToInt ( NEWTIME > NOWTIME ) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 229;
                    NOWTIME = (ushort) ( Functions.GetHSeconds() ) ; 
                    __context__.SourceCodeLine = 228;
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 232;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NEWTIME < NOWTIME ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 233;
                    OLDTIME = (ushort) ( NOWTIME ) ; 
                    __context__.SourceCodeLine = 234;
                    while ( Functions.TestForTrue  ( ( Functions.BoolToInt (GI_ALLOWCMD == 0))  ) ) 
                        { 
                        __context__.SourceCodeLine = 235;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( NOWTIME > NEWTIME ) ) && Functions.TestForTrue ( Functions.BoolToInt ( NOWTIME < OLDTIME ) )) ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 236;
                            GI_ALLOWCMD = (ushort) ( 1 ) ; 
                            } 
                        
                        __context__.SourceCodeLine = 238;
                        NOWTIME = (ushort) ( Functions.GetHSeconds() ) ; 
                        __context__.SourceCodeLine = 234;
                        } 
                    
                    } 
                
                }
            
            __context__.SourceCodeLine = 242;
            GI_ALLOWCMD = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 243;
            SHOWCOMMANDS (  __context__  ) ; 
            
            }
            
        private void SETRECORDERSTATE (  SplusExecutionContext __context__, CrestronString SSTATE ) 
            { 
            
            __context__.SourceCodeLine = 252;
            _SplusNVRAM.GS_RECORDERSTATE  .UpdateValue ( SSTATE  ) ; 
            __context__.SourceCodeLine = 253;
            RECORDERSTATE  .UpdateValue ( _SplusNVRAM.GS_RECORDERSTATE  ) ; 
            __context__.SourceCodeLine = 256;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "Disconnected"))  ) ) 
                { 
                __context__.SourceCodeLine = 256;
                GI_ALLOWCMD = (ushort) ( 0 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 257;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "Dormant"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 257;
                    GI_ALLOWCMD = (ushort) ( 0 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 258;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "Faulted"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 258;
                        GI_ALLOWCMD = (ushort) ( 0 ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 259;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "Init"))  ) ) 
                            { 
                            __context__.SourceCodeLine = 259;
                            GI_ALLOWCMD = (ushort) ( 0 ) ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 260;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionAnyToRecording"))  ) ) 
                                { 
                                __context__.SourceCodeLine = 260;
                                GI_ALLOWCMD = (ushort) ( 0 ) ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 261;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionAnyToPause"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 261;
                                    GI_ALLOWCMD = (ushort) ( 0 ) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 262;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionRecordingToPause"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 262;
                                        GI_ALLOWCMD = (ushort) ( 0 ) ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 263;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionRecordingToStop"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 263;
                                            GI_ALLOWCMD = (ushort) ( 0 ) ; 
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 265;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE != _SplusNVRAM.GS_RECORDERSTATE_SIMPLE))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 265;
                                                GI_ALLOWCMD = (ushort) ( 1 ) ; 
                                                } 
                                            
                                            } 
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                }
            
            __context__.SourceCodeLine = 269;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "PreviewingNoNextSchedule"))  ) ) 
                { 
                __context__.SourceCodeLine = 269;
                _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Previewing"  ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 270;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "PreviewingWithNextSchedule"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 270;
                    _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Previewing"  ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 271;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionAnyToRecording"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 271;
                        _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Recording"  ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 272;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionAnyToPause"))  ) ) 
                            { 
                            __context__.SourceCodeLine = 272;
                            _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Recording"  ) ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 273;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionRecordingToPause"))  ) ) 
                                { 
                                __context__.SourceCodeLine = 273;
                                _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Paused"  ) ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 274;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "TransitionRecordingToStop"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 274;
                                    _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Recording"  ) ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 275;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SSTATE == "Stopped"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 275;
                                        _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( "Previewing"  ) ; 
                                        } 
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 276;
                                        _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( SSTATE  ) ; 
                                        } 
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                }
            
            __context__.SourceCodeLine = 278;
            RECORDERSTATESIMPLE  .UpdateValue ( _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  ) ; 
            __context__.SourceCodeLine = 280;
            SHOWCOMMANDS (  __context__  ) ; 
            
            }
            
        private void PARSERECORDINGDATA (  SplusExecutionContext __context__, CrestronString SROOT , CrestronString SFINDTAG , ushort TAG , CrestronString SDATA , ushort QUEUED ) 
            { 
            CrestronString TEMP;
            TEMP  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
            
            ushort I1 = 0;
            ushort I2 = 0;
            
            
            __context__.SourceCodeLine = 291;
            I1 = (ushort) ( (Functions.Length( SFINDTAG ) + 1) ) ; 
            __context__.SourceCodeLine = 292;
            I2 = (ushort) ( (Functions.Length( SDATA ) - I1) ) ; 
            __context__.SourceCodeLine = 293;
            TEMP  .UpdateValue ( Functions.Mid ( SDATA ,  (int) ( I1 ) ,  (int) ( I2 ) )  ) ; 
            __context__.SourceCodeLine = 295;
            if ( Functions.TestForTrue  ( ( QUEUED)  ) ) 
                { 
                __context__.SourceCodeLine = 296;
                if ( Functions.TestForTrue  ( ( Functions.Find( SROOT , SDATA ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 297;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_1__ = ((int)TAG);
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 1) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 298;
                                GS_QUEUED_RECORDING_ID  .UpdateValue ( TEMP  ) ; 
                                __context__.SourceCodeLine = 298;
                                GI_STATUS_COUNT_QUE = (ushort) ( 0 ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 2) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 299;
                                GS_QUEUED_RECORDING_NAME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 3) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 300;
                                GS_QUEUED_RECORDING_STARTTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 4) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 301;
                                GS_QUEUED_RECORDING_ENDTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 5) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 302;
                                GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 6) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 303;
                                GS_QUEUED_RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            } 
                            
                        }
                        
                    
                    } 
                
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 308;
                if ( Functions.TestForTrue  ( ( Functions.Find( SROOT , SDATA ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 309;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_2__ = ((int)TAG);
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 1) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 310;
                                GS_RECORDING_ID  .UpdateValue ( TEMP  ) ; 
                                __context__.SourceCodeLine = 310;
                                GI_STATUS_COUNT_REC = (ushort) ( 0 ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 2) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 311;
                                GS_RECORDING_NAME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 3) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 312;
                                GS_RECORDING_STARTTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 4) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 313;
                                GS_RECORDING_ENDTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 5) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 314;
                                GS_RECORDING_MINUTESUNTILSTARTTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 6) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 315;
                                GS_RECORDING_MINUTESUNTILENDTIME  .UpdateValue ( TEMP  ) ; 
                                } 
                            
                            } 
                            
                        }
                        
                    
                    } 
                
                } 
            
            
            }
            
        private void CHECKRECORDINGDATA (  SplusExecutionContext __context__, CrestronString SROOT , CrestronString SDATA , ushort QUEUED ) 
            { 
            ushort I = 0;
            
            CrestronString FINDTAG;
            FINDTAG  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
            
            
            __context__.SourceCodeLine = 329;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)6; 
            int __FN_FORSTEP_VAL__1 = (int)1; 
            for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 330;
                MakeString ( FINDTAG , "{0}{1}: ", SROOT , GS_RECORDING_TAG [ I ] ) ; 
                __context__.SourceCodeLine = 331;
                if ( Functions.TestForTrue  ( ( Functions.Find( FINDTAG , SDATA ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 331;
                    PARSERECORDINGDATA (  __context__ , SROOT, FINDTAG, (ushort)( I ), SDATA, (ushort)( QUEUED )) ; 
                    } 
                
                __context__.SourceCodeLine = 332;
                Functions.ProcessLogic ( ) ; 
                __context__.SourceCodeLine = 329;
                } 
            
            
            }
            
        private void PARSEDATA (  SplusExecutionContext __context__, CrestronString SDATA ) 
            { 
            
            __context__.SourceCodeLine = 342;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( DEBUG  .UshortValue >= 1 ))  ) ) 
                { 
                __context__.SourceCodeLine = 342;
                Trace( "ParseData: {0}", DEBUGPRINTHEX (  __context__ , SDATA) ) ; 
                } 
            
            __context__.SourceCodeLine = 347;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Disconnected OK\u000A"))  ) ) 
                { 
                __context__.SourceCodeLine = 347;
                SETRECORDERSTATE (  __context__ , "Disconnected") ; 
                } 
            
            __context__.SourceCodeLine = 349;
            if ( Functions.TestForTrue  ( ( Functions.Find( "Recorder" , SDATA ))  ) ) 
                { 
                __context__.SourceCodeLine = 350;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderPreviewingNoNextSchedule OK\u000A"))  ) ) 
                    { 
                    __context__.SourceCodeLine = 350;
                    SETRECORDERSTATE (  __context__ , "PreviewingNoNextSchedule") ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 351;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderPreviewingWithNextSchedule OK\u000A"))  ) ) 
                        { 
                        __context__.SourceCodeLine = 351;
                        SETRECORDERSTATE (  __context__ , "PreviewingWithNextSchedule") ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 352;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderRecording OK\u000A"))  ) ) 
                            { 
                            __context__.SourceCodeLine = 352;
                            SETRECORDERSTATE (  __context__ , "Recording") ; 
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 353;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderPaused OK\u000A"))  ) ) 
                                { 
                                __context__.SourceCodeLine = 353;
                                SETRECORDERSTATE (  __context__ , "Paused") ; 
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 354;
                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderStopped OK\u000A"))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 354;
                                    SETRECORDERSTATE (  __context__ , "Stopped") ; 
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 355;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderDormant OK\u000A"))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 355;
                                        SETRECORDERSTATE (  __context__ , "Dormant") ; 
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 356;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderFaulted OK\u000A"))  ) ) 
                                            { 
                                            __context__.SourceCodeLine = 356;
                                            SETRECORDERSTATE (  __context__ , "Faulted") ; 
                                            } 
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 357;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderDisconnected OK\u000A"))  ) ) 
                                                { 
                                                __context__.SourceCodeLine = 357;
                                                SETRECORDERSTATE (  __context__ , "Disconnected") ; 
                                                } 
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 359;
                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Init\u000A"))  ) ) 
                                                    { 
                                                    __context__.SourceCodeLine = 359;
                                                    SETRECORDERSTATE (  __context__ , "Init") ; 
                                                    } 
                                                
                                                else 
                                                    {
                                                    __context__.SourceCodeLine = 360;
                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: PreviewingNoNextSchedule\u000A"))  ) ) 
                                                        { 
                                                        __context__.SourceCodeLine = 360;
                                                        SETRECORDERSTATE (  __context__ , "PreviewingNoNextSchedule") ; 
                                                        } 
                                                    
                                                    else 
                                                        {
                                                        __context__.SourceCodeLine = 361;
                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: PreviewingWithNextSchedule\u000A"))  ) ) 
                                                            { 
                                                            __context__.SourceCodeLine = 361;
                                                            SETRECORDERSTATE (  __context__ , "PreviewingWithNextSchedule") ; 
                                                            } 
                                                        
                                                        else 
                                                            {
                                                            __context__.SourceCodeLine = 362;
                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: TransitionAnyToRecording\u000A"))  ) ) 
                                                                { 
                                                                __context__.SourceCodeLine = 362;
                                                                SETRECORDERSTATE (  __context__ , "TransitionAnyToRecording") ; 
                                                                } 
                                                            
                                                            else 
                                                                {
                                                                __context__.SourceCodeLine = 363;
                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Recording\u000A"))  ) ) 
                                                                    { 
                                                                    __context__.SourceCodeLine = 363;
                                                                    SETRECORDERSTATE (  __context__ , "Recording") ; 
                                                                    } 
                                                                
                                                                else 
                                                                    {
                                                                    __context__.SourceCodeLine = 364;
                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: TransitionRecordingToPause\u000A"))  ) ) 
                                                                        { 
                                                                        __context__.SourceCodeLine = 364;
                                                                        SETRECORDERSTATE (  __context__ , "TransitionRecordingToPause") ; 
                                                                        } 
                                                                    
                                                                    else 
                                                                        {
                                                                        __context__.SourceCodeLine = 365;
                                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Paused\u000A"))  ) ) 
                                                                            { 
                                                                            __context__.SourceCodeLine = 365;
                                                                            SETRECORDERSTATE (  __context__ , "Paused") ; 
                                                                            } 
                                                                        
                                                                        else 
                                                                            {
                                                                            __context__.SourceCodeLine = 366;
                                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: TransitionPausedToStop\u000A"))  ) ) 
                                                                                { 
                                                                                __context__.SourceCodeLine = 366;
                                                                                SETRECORDERSTATE (  __context__ , "TransitionPausedToStop") ; 
                                                                                } 
                                                                            
                                                                            else 
                                                                                {
                                                                                __context__.SourceCodeLine = 367;
                                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: TransitionRecordingToStop\u000A"))  ) ) 
                                                                                    { 
                                                                                    __context__.SourceCodeLine = 367;
                                                                                    SETRECORDERSTATE (  __context__ , "TransitionRecordingToStop") ; 
                                                                                    } 
                                                                                
                                                                                else 
                                                                                    {
                                                                                    __context__.SourceCodeLine = 368;
                                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Stopped\u000A"))  ) ) 
                                                                                        { 
                                                                                        __context__.SourceCodeLine = 368;
                                                                                        SETRECORDERSTATE (  __context__ , "Stopped") ; 
                                                                                        } 
                                                                                    
                                                                                    else 
                                                                                        {
                                                                                        __context__.SourceCodeLine = 369;
                                                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Dormant\u000A"))  ) ) 
                                                                                            { 
                                                                                            __context__.SourceCodeLine = 369;
                                                                                            SETRECORDERSTATE (  __context__ , "Dormant") ; 
                                                                                            } 
                                                                                        
                                                                                        else 
                                                                                            {
                                                                                            __context__.SourceCodeLine = 370;
                                                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Faulted\u000A"))  ) ) 
                                                                                                { 
                                                                                                __context__.SourceCodeLine = 370;
                                                                                                SETRECORDERSTATE (  __context__ , "Faulted") ; 
                                                                                                } 
                                                                                            
                                                                                            else 
                                                                                                {
                                                                                                __context__.SourceCodeLine = 371;
                                                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: Disconnected\u000A"))  ) ) 
                                                                                                    { 
                                                                                                    __context__.SourceCodeLine = 371;
                                                                                                    SETRECORDERSTATE (  __context__ , "Disconnected") ; 
                                                                                                    } 
                                                                                                
                                                                                                else 
                                                                                                    {
                                                                                                    __context__.SourceCodeLine = 374;
                                                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderPreviewing OK\u000A"))  ) ) 
                                                                                                        { 
                                                                                                        __context__.SourceCodeLine = 374;
                                                                                                        SETRECORDERSTATE (  __context__ , "PreviewingNoNextSchedule") ; 
                                                                                                        } 
                                                                                                    
                                                                                                    else 
                                                                                                        {
                                                                                                        __context__.SourceCodeLine = 375;
                                                                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderPreviewingQueued OK\u000A"))  ) ) 
                                                                                                            { 
                                                                                                            __context__.SourceCodeLine = 375;
                                                                                                            SETRECORDERSTATE (  __context__ , "PreviewingWithNextSchedule") ; 
                                                                                                            } 
                                                                                                        
                                                                                                        else 
                                                                                                            {
                                                                                                            __context__.SourceCodeLine = 376;
                                                                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "RecorderRunning OK\u000A"))  ) ) 
                                                                                                                { 
                                                                                                                __context__.SourceCodeLine = 376;
                                                                                                                SETRECORDERSTATE (  __context__ , "Dormant") ; 
                                                                                                                } 
                                                                                                            
                                                                                                            else 
                                                                                                                {
                                                                                                                __context__.SourceCodeLine = 379;
                                                                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRDisconnected\u000A"))  ) ) 
                                                                                                                    { 
                                                                                                                    __context__.SourceCodeLine = 379;
                                                                                                                    SETRECORDERSTATE (  __context__ , "Disconnected") ; 
                                                                                                                    } 
                                                                                                                
                                                                                                                else 
                                                                                                                    {
                                                                                                                    __context__.SourceCodeLine = 380;
                                                                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRPreviewing\u000A"))  ) ) 
                                                                                                                        { 
                                                                                                                        __context__.SourceCodeLine = 380;
                                                                                                                        SETRECORDERSTATE (  __context__ , "PreviewingNoNextSchedule") ; 
                                                                                                                        } 
                                                                                                                    
                                                                                                                    else 
                                                                                                                        {
                                                                                                                        __context__.SourceCodeLine = 381;
                                                                                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRPreviewingQueued\u000A"))  ) ) 
                                                                                                                            { 
                                                                                                                            __context__.SourceCodeLine = 381;
                                                                                                                            SETRECORDERSTATE (  __context__ , "PreviewingWithNextSchedule") ; 
                                                                                                                            } 
                                                                                                                        
                                                                                                                        else 
                                                                                                                            {
                                                                                                                            __context__.SourceCodeLine = 382;
                                                                                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRRecording\u000A"))  ) ) 
                                                                                                                                { 
                                                                                                                                __context__.SourceCodeLine = 382;
                                                                                                                                SETRECORDERSTATE (  __context__ , "Recording") ; 
                                                                                                                                } 
                                                                                                                            
                                                                                                                            else 
                                                                                                                                {
                                                                                                                                __context__.SourceCodeLine = 383;
                                                                                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRPaused\u000A"))  ) ) 
                                                                                                                                    { 
                                                                                                                                    __context__.SourceCodeLine = 383;
                                                                                                                                    SETRECORDERSTATE (  __context__ , "Paused") ; 
                                                                                                                                    } 
                                                                                                                                
                                                                                                                                else 
                                                                                                                                    {
                                                                                                                                    __context__.SourceCodeLine = 384;
                                                                                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRStopped\u000A"))  ) ) 
                                                                                                                                        { 
                                                                                                                                        __context__.SourceCodeLine = 384;
                                                                                                                                        SETRECORDERSTATE (  __context__ , "Stopped") ; 
                                                                                                                                        } 
                                                                                                                                    
                                                                                                                                    else 
                                                                                                                                        {
                                                                                                                                        __context__.SourceCodeLine = 385;
                                                                                                                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRRunning\u000A"))  ) ) 
                                                                                                                                            { 
                                                                                                                                            __context__.SourceCodeLine = 385;
                                                                                                                                            SETRECORDERSTATE (  __context__ , "Dormant") ; 
                                                                                                                                            } 
                                                                                                                                        
                                                                                                                                        else 
                                                                                                                                            {
                                                                                                                                            __context__.SourceCodeLine = 386;
                                                                                                                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRFaulted\u000A"))  ) ) 
                                                                                                                                                { 
                                                                                                                                                __context__.SourceCodeLine = 386;
                                                                                                                                                SETRECORDERSTATE (  __context__ , "Faulted") ; 
                                                                                                                                                } 
                                                                                                                                            
                                                                                                                                            else 
                                                                                                                                                {
                                                                                                                                                __context__.SourceCodeLine = 387;
                                                                                                                                                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRStoppingPaused\u000A"))  ) ) 
                                                                                                                                                    { 
                                                                                                                                                    __context__.SourceCodeLine = 387;
                                                                                                                                                    SETRECORDERSTATE (  __context__ , "TransitionPausedToStop") ; 
                                                                                                                                                    } 
                                                                                                                                                
                                                                                                                                                else 
                                                                                                                                                    {
                                                                                                                                                    __context__.SourceCodeLine = 388;
                                                                                                                                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SDATA == "Recorder-Status: RRStoppingRecord\u000A"))  ) ) 
                                                                                                                                                        { 
                                                                                                                                                        __context__.SourceCodeLine = 388;
                                                                                                                                                        SETRECORDERSTATE (  __context__ , "TransitionRecordingToStop") ; 
                                                                                                                                                        } 
                                                                                                                                                    
                                                                                                                                                    }
                                                                                                                                                
                                                                                                                                                }
                                                                                                                                            
                                                                                                                                            }
                                                                                                                                        
                                                                                                                                        }
                                                                                                                                    
                                                                                                                                    }
                                                                                                                                
                                                                                                                                }
                                                                                                                            
                                                                                                                            }
                                                                                                                        
                                                                                                                        }
                                                                                                                    
                                                                                                                    }
                                                                                                                
                                                                                                                }
                                                                                                            
                                                                                                            }
                                                                                                        
                                                                                                        }
                                                                                                    
                                                                                                    }
                                                                                                
                                                                                                }
                                                                                            
                                                                                            }
                                                                                        
                                                                                        }
                                                                                    
                                                                                    }
                                                                                
                                                                                }
                                                                            
                                                                            }
                                                                        
                                                                        }
                                                                    
                                                                    }
                                                                
                                                                }
                                                            
                                                            }
                                                        
                                                        }
                                                    
                                                    }
                                                
                                                }
                                            
                                            }
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    }
                
                } 
            
            __context__.SourceCodeLine = 395;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.Find( "CurrentRecording-" , SDATA ) ) && Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Recording") ) || Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Paused") )) ) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 396;
                CHECKRECORDINGDATA (  __context__ , "CurrentRecording-", SDATA, (ushort)( 0 )) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 399;
                if ( Functions.TestForTrue  ( ( Functions.Find( "NextRecording-" , SDATA ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 400;
                    CHECKRECORDINGDATA (  __context__ , "NextRecording-", SDATA, (ushort)( 1 )) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 403;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.Find( "Recording-" , SDATA ) ) && Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Recording") ) || Functions.TestForTrue ( Functions.BoolToInt (_SplusNVRAM.GS_RECORDERSTATE_SIMPLE == "Paused") )) ) )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 404;
                        CHECKRECORDINGDATA (  __context__ , "Recording-", SDATA, (ushort)( 0 )) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 407;
                        if ( Functions.TestForTrue  ( ( Functions.Find( "Queued-" , SDATA ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 408;
                            CHECKRECORDINGDATA (  __context__ , "Queued-", SDATA, (ushort)( 1 )) ; 
                            } 
                        
                        }
                    
                    }
                
                }
            
            
            }
            
        object PANOPTO_SERIAL_RX_OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                ushort NEXTCHAR = 0;
                
                
                __context__.SourceCodeLine = 422;
                do 
                    { 
                    __context__.SourceCodeLine = 423;
                    NEXTCHAR = (ushort) ( Functions.GetC( PANOPTO_SERIAL_RX ) ) ; 
                    __context__.SourceCodeLine = 424;
                    GS_TEMP_PANOPTO_SERIAL_RX  .UpdateValue ( GS_TEMP_PANOPTO_SERIAL_RX + Functions.Chr (  (int) ( NEXTCHAR ) )  ) ; 
                    __context__.SourceCodeLine = 425;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NEXTCHAR == 10))  ) ) 
                        { 
                        __context__.SourceCodeLine = 426;
                        PARSEDATA (  __context__ , GS_TEMP_PANOPTO_SERIAL_RX) ; 
                        __context__.SourceCodeLine = 427;
                        GS_TEMP_PANOPTO_SERIAL_RX  .UpdateValue ( ""  ) ; 
                        __context__.SourceCodeLine = 428;
                        Functions.Delay (  (int) ( 1 ) ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 430;
                    Functions.Delay (  (int) ( 1 ) ) ; 
                    } 
                while (false == ( Functions.TestForTrue  (  (int) ( Functions.BoolToInt (Functions.Length( PANOPTO_SERIAL_RX ) == 0) ) ) )); 
                __context__.SourceCodeLine = 432;
                UPDATERECORDINGDATA (  __context__  ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object CMD_START_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 438;
            SENDCOMMAND (  __context__ , "START") ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object CMD_STOP_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 439;
        SENDCOMMAND (  __context__ , "STOP") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CMD_PAUSE_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 440;
        SENDCOMMAND (  __context__ , "PAUSE") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CMD_RESUME_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 441;
        SENDCOMMAND (  __context__ , "RESUME") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CMD_EXTEND_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 442;
        SENDCOMMAND (  __context__ , "EXTEND") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GET_STATUS_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 446;
        PANOPTO_SERIAL_TX  .UpdateValue ( "STATUS\r\n"  ) ; 
        __context__.SourceCodeLine = 447;
        GI_STATUS_COUNT_REC = (ushort) ( (GI_STATUS_COUNT_REC + 1) ) ; 
        __context__.SourceCodeLine = 448;
        GI_STATUS_COUNT_QUE = (ushort) ( (GI_STATUS_COUNT_QUE + 1) ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object ERROR_NOCOM_OnPush_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 453;
        GI_ALLOWCMD = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 454;
        SETRECORDERSTATE (  __context__ , "") ; 
        __context__.SourceCodeLine = 455;
        CLEARRECORDINGDATA (  __context__  ) ; 
        __context__.SourceCodeLine = 456;
        CLEARQUERECORDINGDATA (  __context__  ) ; 
        __context__.SourceCodeLine = 457;
        UPDATERECORDINGDATA (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 467;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 468;
        GI_ALLOWCMD = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 469;
        GS_TEMP_PANOPTO_SERIAL_RX  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 470;
        _SplusNVRAM.GS_RECORDERSTATE  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 471;
        _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 473;
        GI_STATUS_COUNT_REC = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 474;
        GI_STATUS_COUNT_QUE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 476;
        GS_RECORDING_TAG [ 1 ]  .UpdateValue ( "Id"  ) ; 
        __context__.SourceCodeLine = 477;
        GS_RECORDING_TAG [ 2 ]  .UpdateValue ( "Name"  ) ; 
        __context__.SourceCodeLine = 478;
        GS_RECORDING_TAG [ 3 ]  .UpdateValue ( "StartTime"  ) ; 
        __context__.SourceCodeLine = 479;
        GS_RECORDING_TAG [ 4 ]  .UpdateValue ( "EndTime"  ) ; 
        __context__.SourceCodeLine = 480;
        GS_RECORDING_TAG [ 5 ]  .UpdateValue ( "MinutesUntilStartTime"  ) ; 
        __context__.SourceCodeLine = 481;
        GS_RECORDING_TAG [ 6 ]  .UpdateValue ( "MinutesUntilEndTime"  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    GS_TEMP_PANOPTO_SERIAL_RX  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 1000, this );
    _SplusNVRAM.GS_RECORDERSTATE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    _SplusNVRAM.GS_RECORDERSTATE_SIMPLE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    GS_RECORDING_ID  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    GS_RECORDING_NAME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    GS_RECORDING_STARTTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 30, this );
    GS_RECORDING_ENDTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 30, this );
    GS_RECORDING_MINUTESUNTILSTARTTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 10, this );
    GS_RECORDING_MINUTESUNTILENDTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 10, this );
    GS_QUEUED_RECORDING_ID  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    GS_QUEUED_RECORDING_NAME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    GS_QUEUED_RECORDING_STARTTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 30, this );
    GS_QUEUED_RECORDING_ENDTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 30, this );
    GS_QUEUED_RECORDING_MINUTESUNTILSTARTTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 10, this );
    GS_QUEUED_RECORDING_MINUTESUNTILENDTIME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 10, this );
    GS_RECORDING_TAG  = new CrestronString[ 7 ];
    for( uint i = 0; i < 7; i++ )
        GS_RECORDING_TAG [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    
    GET_STATUS = new Crestron.Logos.SplusObjects.DigitalInput( GET_STATUS__DigitalInput__, this );
    m_DigitalInputList.Add( GET_STATUS__DigitalInput__, GET_STATUS );
    
    CMD_START = new Crestron.Logos.SplusObjects.DigitalInput( CMD_START__DigitalInput__, this );
    m_DigitalInputList.Add( CMD_START__DigitalInput__, CMD_START );
    
    CMD_STOP = new Crestron.Logos.SplusObjects.DigitalInput( CMD_STOP__DigitalInput__, this );
    m_DigitalInputList.Add( CMD_STOP__DigitalInput__, CMD_STOP );
    
    CMD_PAUSE = new Crestron.Logos.SplusObjects.DigitalInput( CMD_PAUSE__DigitalInput__, this );
    m_DigitalInputList.Add( CMD_PAUSE__DigitalInput__, CMD_PAUSE );
    
    CMD_RESUME = new Crestron.Logos.SplusObjects.DigitalInput( CMD_RESUME__DigitalInput__, this );
    m_DigitalInputList.Add( CMD_RESUME__DigitalInput__, CMD_RESUME );
    
    CMD_EXTEND = new Crestron.Logos.SplusObjects.DigitalInput( CMD_EXTEND__DigitalInput__, this );
    m_DigitalInputList.Add( CMD_EXTEND__DigitalInput__, CMD_EXTEND );
    
    ERROR_NOCOM = new Crestron.Logos.SplusObjects.DigitalInput( ERROR_NOCOM__DigitalInput__, this );
    m_DigitalInputList.Add( ERROR_NOCOM__DigitalInput__, ERROR_NOCOM );
    
    RECORDING_ABOUTTOSTART = new Crestron.Logos.SplusObjects.DigitalOutput( RECORDING_ABOUTTOSTART__DigitalOutput__, this );
    m_DigitalOutputList.Add( RECORDING_ABOUTTOSTART__DigitalOutput__, RECORDING_ABOUTTOSTART );
    
    SHOW_CMD_START = new Crestron.Logos.SplusObjects.DigitalOutput( SHOW_CMD_START__DigitalOutput__, this );
    m_DigitalOutputList.Add( SHOW_CMD_START__DigitalOutput__, SHOW_CMD_START );
    
    SHOW_CMD_STOP = new Crestron.Logos.SplusObjects.DigitalOutput( SHOW_CMD_STOP__DigitalOutput__, this );
    m_DigitalOutputList.Add( SHOW_CMD_STOP__DigitalOutput__, SHOW_CMD_STOP );
    
    SHOW_CMD_PAUSE = new Crestron.Logos.SplusObjects.DigitalOutput( SHOW_CMD_PAUSE__DigitalOutput__, this );
    m_DigitalOutputList.Add( SHOW_CMD_PAUSE__DigitalOutput__, SHOW_CMD_PAUSE );
    
    SHOW_CMD_RESUME = new Crestron.Logos.SplusObjects.DigitalOutput( SHOW_CMD_RESUME__DigitalOutput__, this );
    m_DigitalOutputList.Add( SHOW_CMD_RESUME__DigitalOutput__, SHOW_CMD_RESUME );
    
    SHOW_CMD_EXTEND = new Crestron.Logos.SplusObjects.DigitalOutput( SHOW_CMD_EXTEND__DigitalOutput__, this );
    m_DigitalOutputList.Add( SHOW_CMD_EXTEND__DigitalOutput__, SHOW_CMD_EXTEND );
    
    HAS_RECORDING_DATA = new Crestron.Logos.SplusObjects.DigitalOutput( HAS_RECORDING_DATA__DigitalOutput__, this );
    m_DigitalOutputList.Add( HAS_RECORDING_DATA__DigitalOutput__, HAS_RECORDING_DATA );
    
    HAS_QUEUED_DATA = new Crestron.Logos.SplusObjects.DigitalOutput( HAS_QUEUED_DATA__DigitalOutput__, this );
    m_DigitalOutputList.Add( HAS_QUEUED_DATA__DigitalOutput__, HAS_QUEUED_DATA );
    
    ENABLE_ADHOC_START = new Crestron.Logos.SplusObjects.AnalogInput( ENABLE_ADHOC_START__AnalogSerialInput__, this );
    m_AnalogInputList.Add( ENABLE_ADHOC_START__AnalogSerialInput__, ENABLE_ADHOC_START );
    
    DEBUG = new Crestron.Logos.SplusObjects.AnalogInput( DEBUG__AnalogSerialInput__, this );
    m_AnalogInputList.Add( DEBUG__AnalogSerialInput__, DEBUG );
    
    PANOPTO_SERIAL_TX = new Crestron.Logos.SplusObjects.StringOutput( PANOPTO_SERIAL_TX__AnalogSerialOutput__, this );
    m_StringOutputList.Add( PANOPTO_SERIAL_TX__AnalogSerialOutput__, PANOPTO_SERIAL_TX );
    
    RECORDERSTATE = new Crestron.Logos.SplusObjects.StringOutput( RECORDERSTATE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDERSTATE__AnalogSerialOutput__, RECORDERSTATE );
    
    RECORDERSTATESIMPLE = new Crestron.Logos.SplusObjects.StringOutput( RECORDERSTATESIMPLE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDERSTATESIMPLE__AnalogSerialOutput__, RECORDERSTATESIMPLE );
    
    RECORDING_ID = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_ID__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_ID__AnalogSerialOutput__, RECORDING_ID );
    
    RECORDING_NAME = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_NAME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_NAME__AnalogSerialOutput__, RECORDING_NAME );
    
    RECORDING_STARTTIME = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_STARTTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_STARTTIME__AnalogSerialOutput__, RECORDING_STARTTIME );
    
    RECORDING_ENDTIME = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_ENDTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_ENDTIME__AnalogSerialOutput__, RECORDING_ENDTIME );
    
    RECORDING_MINUTESUNTILSTARTTIME = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__, RECORDING_MINUTESUNTILSTARTTIME );
    
    RECORDING_MINUTESUNTILENDTIME = new Crestron.Logos.SplusObjects.StringOutput( RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__, RECORDING_MINUTESUNTILENDTIME );
    
    QUEUED_RECORDING_ID = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_ID__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_ID__AnalogSerialOutput__, QUEUED_RECORDING_ID );
    
    QUEUED_RECORDING_NAME = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_NAME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_NAME__AnalogSerialOutput__, QUEUED_RECORDING_NAME );
    
    QUEUED_RECORDING_STARTTIME = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_STARTTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_STARTTIME__AnalogSerialOutput__, QUEUED_RECORDING_STARTTIME );
    
    QUEUED_RECORDING_ENDTIME = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_ENDTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_ENDTIME__AnalogSerialOutput__, QUEUED_RECORDING_ENDTIME );
    
    QUEUED_RECORDING_MINUTESUNTILSTARTTIME = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__, QUEUED_RECORDING_MINUTESUNTILSTARTTIME );
    
    QUEUED_RECORDING_MINUTESUNTILENDTIME = new Crestron.Logos.SplusObjects.StringOutput( QUEUED_RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( QUEUED_RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__, QUEUED_RECORDING_MINUTESUNTILENDTIME );
    
    PANOPTO_SERIAL_RX = new Crestron.Logos.SplusObjects.BufferInput( PANOPTO_SERIAL_RX__AnalogSerialInput__, 1000, this );
    m_StringInputList.Add( PANOPTO_SERIAL_RX__AnalogSerialInput__, PANOPTO_SERIAL_RX );
    
    
    PANOPTO_SERIAL_RX.OnSerialChange.Add( new InputChangeHandlerWrapper( PANOPTO_SERIAL_RX_OnChange_0, true ) );
    CMD_START.OnDigitalPush.Add( new InputChangeHandlerWrapper( CMD_START_OnPush_1, false ) );
    CMD_STOP.OnDigitalPush.Add( new InputChangeHandlerWrapper( CMD_STOP_OnPush_2, false ) );
    CMD_PAUSE.OnDigitalPush.Add( new InputChangeHandlerWrapper( CMD_PAUSE_OnPush_3, false ) );
    CMD_RESUME.OnDigitalPush.Add( new InputChangeHandlerWrapper( CMD_RESUME_OnPush_4, false ) );
    CMD_EXTEND.OnDigitalPush.Add( new InputChangeHandlerWrapper( CMD_EXTEND_OnPush_5, false ) );
    GET_STATUS.OnDigitalPush.Add( new InputChangeHandlerWrapper( GET_STATUS_OnPush_6, false ) );
    ERROR_NOCOM.OnDigitalPush.Add( new InputChangeHandlerWrapper( ERROR_NOCOM_OnPush_7, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_UW_PANOPTO_SERIAL_PROCESSOR_V2_0 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint GET_STATUS__DigitalInput__ = 0;
const uint CMD_START__DigitalInput__ = 1;
const uint CMD_STOP__DigitalInput__ = 2;
const uint CMD_PAUSE__DigitalInput__ = 3;
const uint CMD_RESUME__DigitalInput__ = 4;
const uint CMD_EXTEND__DigitalInput__ = 5;
const uint ERROR_NOCOM__DigitalInput__ = 6;
const uint RECORDING_ABOUTTOSTART__DigitalOutput__ = 0;
const uint PANOPTO_SERIAL_RX__AnalogSerialInput__ = 0;
const uint ENABLE_ADHOC_START__AnalogSerialInput__ = 1;
const uint DEBUG__AnalogSerialInput__ = 2;
const uint SHOW_CMD_START__DigitalOutput__ = 1;
const uint SHOW_CMD_STOP__DigitalOutput__ = 2;
const uint SHOW_CMD_PAUSE__DigitalOutput__ = 3;
const uint SHOW_CMD_RESUME__DigitalOutput__ = 4;
const uint SHOW_CMD_EXTEND__DigitalOutput__ = 5;
const uint HAS_RECORDING_DATA__DigitalOutput__ = 6;
const uint HAS_QUEUED_DATA__DigitalOutput__ = 7;
const uint PANOPTO_SERIAL_TX__AnalogSerialOutput__ = 0;
const uint RECORDERSTATE__AnalogSerialOutput__ = 1;
const uint RECORDERSTATESIMPLE__AnalogSerialOutput__ = 2;
const uint RECORDING_ID__AnalogSerialOutput__ = 3;
const uint RECORDING_NAME__AnalogSerialOutput__ = 4;
const uint RECORDING_STARTTIME__AnalogSerialOutput__ = 5;
const uint RECORDING_ENDTIME__AnalogSerialOutput__ = 6;
const uint RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__ = 7;
const uint RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__ = 8;
const uint QUEUED_RECORDING_ID__AnalogSerialOutput__ = 9;
const uint QUEUED_RECORDING_NAME__AnalogSerialOutput__ = 10;
const uint QUEUED_RECORDING_STARTTIME__AnalogSerialOutput__ = 11;
const uint QUEUED_RECORDING_ENDTIME__AnalogSerialOutput__ = 12;
const uint QUEUED_RECORDING_MINUTESUNTILSTARTTIME__AnalogSerialOutput__ = 13;
const uint QUEUED_RECORDING_MINUTESUNTILENDTIME__AnalogSerialOutput__ = 14;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    [SplusStructAttribute(0, false, true)]
            public CrestronString GS_RECORDERSTATE;
            [SplusStructAttribute(1, false, true)]
            public CrestronString GS_RECORDERSTATE_SIMPLE;
            
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
