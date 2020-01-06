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

namespace CrestronModule_PHOTOCELL_PROPORTIONAL_CONTROL_V1_7
{
    public class CrestronModuleClass_PHOTOCELL_PROPORTIONAL_CONTROL_V1_7 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput DISABLE;
        Crestron.Logos.SplusObjects.DigitalInput DISABLEANDOFF;
        Crestron.Logos.SplusObjects.DigitalInput RESTART_PHOTOCELL;
        Crestron.Logos.SplusObjects.DigitalInput RESTARTPHOTOCELLWITHRESPONSETIME;
        Crestron.Logos.SplusObjects.DigitalInput CALIBRATE_NIGHT;
        Crestron.Logos.SplusObjects.DigitalInput FASTER_RESPONSE;
        Crestron.Logos.SplusObjects.DigitalInput SLOWER_RESPONSE;
        Crestron.Logos.SplusObjects.DigitalInput DIMLEVELMINCHANGED;
        Crestron.Logos.SplusObjects.AnalogInput SENSITIVITYLEVEL;
        Crestron.Logos.SplusObjects.DigitalInput MIN_DIM_LEVEL_CHANGED;
        Crestron.Logos.SplusObjects.DigitalInput TEMPRAISE;
        Crestron.Logos.SplusObjects.DigitalInput TEMPLOWER;
        Crestron.Logos.SplusObjects.AnalogInput SENSOR_INPUT;
        Crestron.Logos.SplusObjects.DigitalOutput DISABLED;
        Crestron.Logos.SplusObjects.DigitalOutput SEND_RAMP_1;
        Crestron.Logos.SplusObjects.DigitalOutput SEND_RAMP_2;
        Crestron.Logos.SplusObjects.DigitalOutput DALI_OFF;
        Crestron.Logos.SplusObjects.AnalogOutput DIM_LEVEL_OUT;
        Crestron.Logos.SplusObjects.AnalogOutput TARGET_DIM_LEVEL;
        Crestron.Logos.SplusObjects.AnalogOutput DIM_FADE_TIME;
        Crestron.Logos.SplusObjects.AnalogOutput NIGHTTIME_SETPOINT;
        Crestron.Logos.SplusObjects.AnalogOutput NIGHTTIME_DIM_OUT;
        Crestron.Logos.SplusObjects.AnalogOutput RESPONSE_TIME;
        Crestron.Logos.SplusObjects.AnalogOutput SENSITIVITYLEVELOUT;
        Crestron.Logos.SplusObjects.AnalogOutput MIN_DIM_LEVEL_OUT;
        Crestron.Logos.SplusObjects.AnalogOutput DIMOUTPUTMINCHANGE;
        UShortParameter RESTART_RAMP_TIME;
        UShortParameter NIGHTTIME_SETPOINT_DEFAULT;
        UShortParameter OFFFADETIME;
        StringParameter FILELOCATION;
        UShortParameter ID;
        ushort G_OUTPUTVALUE = 0;
        ushort G_SEMAPHORE = 0;
        ushort G_SENDRAMPSEMAPHORE = 0;
        ushort G_BSENSORINPUTSEMAPHORE = 0;
        ushort G_INITIALIZED = 0;
        ushort G_BDISABLESENSOR = 0;
        ushort G_IFILEHANDLE = 0;
        int G_PREVIOUSTARGET = 0;
        int G_LOFFSET = 0;
        CrestronString G_FILEPATH;
        AUTOLEVEL AUTOSETPOINTDATA;
        private void SENDRAMPALWAYS (  SplusExecutionContext __context__,  uint FADETIME ,  int RAMPVALUE ) 
            { 
            
            __context__.SourceCodeLine = 141;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( RAMPVALUE > 65535 ))  ) ) 
                {
                __context__.SourceCodeLine = 142;
                RAMPVALUE = (int) ( 65535 ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 143;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( RAMPVALUE < 0 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 144;
                    RAMPVALUE = (int) ( 0 ) ; 
                    }
                
                }
            
            __context__.SourceCodeLine = 152;
            TARGET_DIM_LEVEL  .Value = (ushort) ( RAMPVALUE ) ; 
            __context__.SourceCodeLine = 153;
            DIM_FADE_TIME  .Value = (ushort) ( FADETIME ) ; 
            __context__.SourceCodeLine = 155;
            G_PREVIOUSTARGET = (int) ( RAMPVALUE ) ; 
            __context__.SourceCodeLine = 156;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SEND_RAMP_1  .Value == 0))  ) ) 
                { 
                __context__.SourceCodeLine = 158;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TARGET_DIM_LEVEL  .Value == 0))  ) ) 
                    { 
                    __context__.SourceCodeLine = 160;
                    DALI_OFF  .Value = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 161;
                    DALI_OFF  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 163;
                SEND_RAMP_2  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 164;
                SEND_RAMP_1  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 168;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TARGET_DIM_LEVEL  .Value == 0))  ) ) 
                    { 
                    __context__.SourceCodeLine = 170;
                    DALI_OFF  .Value = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 171;
                    DALI_OFF  .Value = (ushort) ( 0 ) ; 
                    } 
                
                __context__.SourceCodeLine = 173;
                SEND_RAMP_1  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 174;
                SEND_RAMP_2  .Value = (ushort) ( 1 ) ; 
                } 
            
            
            }
            
        private void SENDRAMP (  SplusExecutionContext __context__,  uint FADETIME ,  int RAMPVALUE ) 
            { 
            int LDIFF = 0;
            
            
            __context__.SourceCodeLine = 187;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Functions.High( (ushort) DIM_LEVEL_OUT  .Value ) == Functions.High( (ushort) RAMPVALUE )))  ) ) 
                {
                __context__.SourceCodeLine = 188;
                return ; 
                }
            
            __context__.SourceCodeLine = 190;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (RAMPVALUE - G_PREVIOUSTARGET) >= 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 191;
                LDIFF = (int) ( (RAMPVALUE - G_PREVIOUSTARGET) ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 193;
                LDIFF = (int) ( (G_PREVIOUSTARGET - RAMPVALUE) ) ; 
                }
            
            __context__.SourceCodeLine = 195;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( LDIFF < DIMOUTPUTMINCHANGE  .Value ) ) && Functions.TestForTrue ( Functions.BoolToInt (RAMPVALUE != 0) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 196;
                return ; 
                }
            
            __context__.SourceCodeLine = 197;
            SENDRAMPALWAYS (  __context__ , (uint)( FADETIME ), (int)( RAMPVALUE )) ; 
            
            }
            
        
        
        private ushort SAVEFILENOW (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 236;
            StartFileOperations ( ) ; 
            __context__.SourceCodeLine = 237;
            G_IFILEHANDLE = (ushort) ( FileOpen( G_FILEPATH ,(ushort) (((1 | 256) | 512) | 32768) ) ) ; 
            __context__.SourceCodeLine = 238;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( G_IFILEHANDLE >= 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 239;
                WriteStructure (  (short) ( G_IFILEHANDLE ) , AUTOSETPOINTDATA ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 241;
                GenerateUserError ( "File Save Error {0:d} Saving file: '{1}'.\r\n", (int)G_IFILEHANDLE, G_FILEPATH ) ; 
                }
            
            __context__.SourceCodeLine = 243;
            FileClose (  (short) ( G_IFILEHANDLE ) ) ; 
            __context__.SourceCodeLine = 245;
            EndFileOperations ( ) ; 
            __context__.SourceCodeLine = 246;
            return (ushort)( 1) ; 
            
            }
            
        private ushort SAVEFILE (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 251;
            CreateWait ( "SAVEWAIT" , 200 , SAVEWAIT_Callback ) ;
            __context__.SourceCodeLine = 264;
            RetimeWait ( (int)200, "SAVEWAIT" ) ; 
            __context__.SourceCodeLine = 265;
            return (ushort)( 1) ; 
            
            }
            
        public void SAVEWAIT_CallbackFn( object stateInfo )
        {
        
            try
            {
                Wait __LocalWait__ = (Wait)stateInfo;
                SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
                __LocalWait__.RemoveFromList();
                
            
            __context__.SourceCodeLine = 257;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (G_SEMAPHORE == 0))  ) ) 
                { 
                __context__.SourceCodeLine = 259;
                G_SEMAPHORE = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 260;
                SAVEFILENOW (  __context__  ) ; 
                __context__.SourceCodeLine = 261;
                G_SEMAPHORE = (ushort) ( 0 ) ; 
                } 
            
            
        
        
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            
        }
        
    object DIMLEVELMINCHANGED_OnPush_0 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 270;
            AUTOSETPOINTDATA . DIMLEVELMINCHANGE = (ushort) ( DIMOUTPUTMINCHANGE  .Value ) ; 
            __context__.SourceCodeLine = 271;
            SAVEFILE (  __context__  ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
private void LOADDEFAULTVALUES (  SplusExecutionContext __context__ ) 
    { 
    
    __context__.SourceCodeLine = 276;
    AUTOSETPOINTDATA . AUTO_X = (ushort) ( 32767 ) ; 
    __context__.SourceCodeLine = 277;
    AUTOSETPOINTDATA . AUTO_Y = (ushort) ( 0 ) ; 
    __context__.SourceCodeLine = 278;
    AUTOSETPOINTDATA . SENSITIVITY_LEVEL = (ushort) ( 32767 ) ; 
    __context__.SourceCodeLine = 279;
    AUTOSETPOINTDATA . NIGHTTIME_SETPOINT = (ushort) ( NIGHTTIME_SETPOINT_DEFAULT  .Value ) ; 
    __context__.SourceCodeLine = 280;
    AUTOSETPOINTDATA . NIGHTTIME_DIM_LEVEL = (int) ( 65535 ) ; 
    __context__.SourceCodeLine = 281;
    AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 3000 ) ; 
    __context__.SourceCodeLine = 282;
    AUTOSETPOINTDATA . SLOPEVALUE = (uint) ( 100 ) ; 
    __context__.SourceCodeLine = 283;
    AUTOSETPOINTDATA . YINTERCEPT = (uint) ( 65535 ) ; 
    __context__.SourceCodeLine = 284;
    AUTOSETPOINTDATA . MINDIMLEVEL = (int) ( 6554 ) ; 
    __context__.SourceCodeLine = 285;
    AUTOSETPOINTDATA . ENABLED = (ushort) ( 0 ) ; 
    __context__.SourceCodeLine = 286;
    AUTOSETPOINTDATA . DIMLEVELMINCHANGE = (ushort) ( 1965 ) ; 
    __context__.SourceCodeLine = 287;
    DIMOUTPUTMINCHANGE  .Value = (ushort) ( AUTOSETPOINTDATA.DIMLEVELMINCHANGE ) ; 
    __context__.SourceCodeLine = 288;
    RESPONSE_TIME  .Value = (ushort) ( AUTOSETPOINTDATA.RESPONSE_TIME ) ; 
    __context__.SourceCodeLine = 289;
    NIGHTTIME_SETPOINT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ) ; 
    __context__.SourceCodeLine = 290;
    NIGHTTIME_DIM_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
    __context__.SourceCodeLine = 291;
    SENSITIVITYLEVELOUT  .Value = (ushort) ( 32767 ) ; 
    __context__.SourceCodeLine = 293;
    DISABLED  .Value = (ushort) ( 1 ) ; 
    
    }
    
private ushort CALCULATEOUTPUT (  SplusExecutionContext __context__,  ushort SENSORINPUT ) 
    { 
    int RETURNVALUE = 0;
    
    
    __context__.SourceCodeLine = 305;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SENSORINPUT <= AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ))  ) ) 
        {
        __context__.SourceCodeLine = 306;
        RETURNVALUE = (int) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
        }
    
    else 
        {
        __context__.SourceCodeLine = 308;
        RETURNVALUE = (int) ( (AUTOSETPOINTDATA.YINTERCEPT - ((AUTOSETPOINTDATA.SLOPEVALUE * (SENSORINPUT - AUTOSETPOINTDATA.NIGHTTIME_SETPOINT)) / 100)) ) ; 
        }
    
    __context__.SourceCodeLine = 310;
    RETURNVALUE = (int) ( (RETURNVALUE + G_LOFFSET) ) ; 
    __context__.SourceCodeLine = 312;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( RETURNVALUE > 65535 ))  ) ) 
        {
        __context__.SourceCodeLine = 313;
        RETURNVALUE = (int) ( 65535 ) ; 
        }
    
    __context__.SourceCodeLine = 315;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( RETURNVALUE <= MIN_DIM_LEVEL_OUT  .Value ) ) || Functions.TestForTrue ( Functions.BoolToInt ( RETURNVALUE <= 0 ) )) ))  ) ) 
        {
        __context__.SourceCodeLine = 316;
        RETURNVALUE = (int) ( MIN_DIM_LEVEL_OUT  .Value ) ; 
        }
    
    __context__.SourceCodeLine = 318;
    
    __context__.SourceCodeLine = 328;
    return (ushort)( RETURNVALUE) ; 
    
    }
    
private void ENABLEPHOTOCELL (  SplusExecutionContext __context__, ushort BMAINTAINOFFSET ,  uint RAMPTIME ) 
    { 
    ushort OUTPUTLEVEL = 0;
    
    ushort BSAVEFILE = 0;
    
    
    __context__.SourceCodeLine = 337;
    NIGHTTIME_SETPOINT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ) ; 
    __context__.SourceCodeLine = 338;
    NIGHTTIME_DIM_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
    __context__.SourceCodeLine = 339;
    MIN_DIM_LEVEL_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.MINDIMLEVEL ) ; 
    __context__.SourceCodeLine = 340;
    SENSITIVITYLEVELOUT  .Value = (ushort) ( AUTOSETPOINTDATA.SENSITIVITY_LEVEL ) ; 
    __context__.SourceCodeLine = 341;
    BSAVEFILE = (ushort) ( 0 ) ; 
    __context__.SourceCodeLine = 344;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.ENABLED == 0))  ) ) 
        { 
        __context__.SourceCodeLine = 346;
        AUTOSETPOINTDATA . ENABLED = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 347;
        DISABLED  .Value = (ushort) ( 0 ) ; 
        } 
    
    __context__.SourceCodeLine = 351;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BMAINTAINOFFSET == 0))  ) ) 
        { 
        __context__.SourceCodeLine = 353;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (G_LOFFSET != 0))  ) ) 
            { 
            __context__.SourceCodeLine = 355;
            G_LOFFSET = (int) ( 0 ) ; 
            __context__.SourceCodeLine = 356;
            BSAVEFILE = (ushort) ( 1 ) ; 
            } 
        
        } 
    
    __context__.SourceCodeLine = 360;
    if ( Functions.TestForTrue  ( ( BSAVEFILE)  ) ) 
        {
        __context__.SourceCodeLine = 361;
        SAVEFILE (  __context__  ) ; 
        }
    
    __context__.SourceCodeLine = 363;
    G_BDISABLESENSOR = (ushort) ( 1 ) ; 
    __context__.SourceCodeLine = 365;
    CancelAllWait ( ) ; 
    __context__.SourceCodeLine = 367;
    OUTPUTLEVEL = (ushort) ( (CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) + G_LOFFSET) ) ; 
    __context__.SourceCodeLine = 371;
    
    __context__.SourceCodeLine = 378;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( OUTPUTLEVEL > AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ))  ) ) 
        {
        __context__.SourceCodeLine = 379;
        OUTPUTLEVEL = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
        }
    
    __context__.SourceCodeLine = 383;
    
    __context__.SourceCodeLine = 387;
    SENDRAMPALWAYS (  __context__ , (uint)( RAMPTIME ), (int)( OUTPUTLEVEL )) ; 
    __context__.SourceCodeLine = 388;
    G_BDISABLESENSOR = (ushort) ( 0 ) ; 
    
    }
    
private ushort LOADFILE (  SplusExecutionContext __context__ ) 
    { 
    FILE_INFO TEMPINFO;
    TEMPINFO  = new FILE_INFO();
    TEMPINFO .PopulateDefaults();
    
    short IFILEHANDLE = 0;
    
    short IFINDRESULT = 0;
    
    
    __context__.SourceCodeLine = 401;
    StartFileOperations ( ) ; 
    __context__.SourceCodeLine = 402;
    IFINDRESULT = (short) ( FindFirst( G_FILEPATH , ref TEMPINFO ) ) ; 
    __context__.SourceCodeLine = 403;
    FindClose ( ) ; 
    __context__.SourceCodeLine = 404;
    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (IFINDRESULT == 0))  ) ) 
        { 
        __context__.SourceCodeLine = 406;
        IFILEHANDLE = (short) ( FileOpen( G_FILEPATH ,(ushort) (0 | 32768) ) ) ; 
        __context__.SourceCodeLine = 407;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( IFILEHANDLE >= 0 ))  ) ) 
            {
            __context__.SourceCodeLine = 408;
            ReadStructure (  (short) ( IFILEHANDLE ) , AUTOSETPOINTDATA ) ; 
            }
        
        else 
            { 
            __context__.SourceCodeLine = 411;
            GenerateUserError ( "File Load Error {0:d} loading file: '{1}'.\r\n", (int)IFILEHANDLE, G_FILEPATH ) ; 
            __context__.SourceCodeLine = 412;
            EndFileOperations ( ) ; 
            __context__.SourceCodeLine = 413;
            LOADDEFAULTVALUES (  __context__  ) ; 
            __context__.SourceCodeLine = 414;
            return (ushort)( 0) ; 
            } 
        
        __context__.SourceCodeLine = 416;
        FileClose (  (short) ( IFILEHANDLE ) ) ; 
        __context__.SourceCodeLine = 418;
        NIGHTTIME_SETPOINT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ) ; 
        __context__.SourceCodeLine = 419;
        NIGHTTIME_DIM_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
        __context__.SourceCodeLine = 420;
        MIN_DIM_LEVEL_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.MINDIMLEVEL ) ; 
        __context__.SourceCodeLine = 421;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.DIMLEVELMINCHANGE == 0))  ) ) 
            {
            __context__.SourceCodeLine = 422;
            AUTOSETPOINTDATA . DIMLEVELMINCHANGE = (ushort) ( 1965 ) ; 
            }
        
        __context__.SourceCodeLine = 424;
        DIMOUTPUTMINCHANGE  .Value = (ushort) ( AUTOSETPOINTDATA.DIMLEVELMINCHANGE ) ; 
        __context__.SourceCodeLine = 425;
        SENSITIVITYLEVELOUT  .Value = (ushort) ( AUTOSETPOINTDATA.SENSITIVITY_LEVEL ) ; 
        __context__.SourceCodeLine = 426;
        RESPONSE_TIME  .Value = (ushort) ( AUTOSETPOINTDATA.RESPONSE_TIME ) ; 
        __context__.SourceCodeLine = 427;
        G_LOFFSET = (int) ( AUTOSETPOINTDATA.OFFSET ) ; 
        __context__.SourceCodeLine = 430;
        AUTOSETPOINTDATA . ENABLED = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 432;
        if ( Functions.TestForTrue  ( ( AUTOSETPOINTDATA.ENABLED)  ) ) 
            { 
            __context__.SourceCodeLine = 434;
            DISABLED  .Value = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 436;
            ENABLEPHOTOCELL (  __context__ , (ushort)( 1 ), (uint)( 50 )) ; 
            } 
        
        else 
            { 
            __context__.SourceCodeLine = 440;
            DISABLED  .Value = (ushort) ( 1 ) ; 
            } 
        
        } 
    
    else 
        { 
        __context__.SourceCodeLine = 449;
        LOADDEFAULTVALUES (  __context__  ) ; 
        } 
    
    __context__.SourceCodeLine = 451;
    EndFileOperations ( ) ; 
    __context__.SourceCodeLine = 453;
    return (ushort)( 1) ; 
    
    }
    
private void CALCULATEXFERCONSTANTS (  SplusExecutionContext __context__ ) 
    { 
    
    __context__.SourceCodeLine = 460;
    AUTOSETPOINTDATA . SLOPEVALUE = (uint) ( (6553500 / (65535 - AUTOSETPOINTDATA.SENSITIVITY_LEVEL)) ) ; 
    __context__.SourceCodeLine = 462;
    AUTOSETPOINTDATA . YINTERCEPT = (uint) ( 65535 ) ; 
    
    }
    
private void INITIALIZE (  SplusExecutionContext __context__ ) 
    { 
    ushort OUTPUTLEVEL = 0;
    
    
    __context__.SourceCodeLine = 506;
    if ( Functions.TestForTrue  ( ( AUTOSETPOINTDATA.ENABLED)  ) ) 
        { 
        __context__.SourceCodeLine = 509;
        CancelAllWait ( ) ; 
        __context__.SourceCodeLine = 511;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SENSOR_INPUT  .UshortValue <= AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ))  ) ) 
            {
            __context__.SourceCodeLine = 512;
            OUTPUTLEVEL = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
            }
        
        else 
            {
            __context__.SourceCodeLine = 514;
            OUTPUTLEVEL = (ushort) ( CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) ) ; 
            }
        
        __context__.SourceCodeLine = 516;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( OUTPUTLEVEL > AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ))  ) ) 
            {
            __context__.SourceCodeLine = 517;
            OUTPUTLEVEL = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
            }
        
        __context__.SourceCodeLine = 519;
        SENDRAMP (  __context__ , (uint)( 50 ), (int)( OUTPUTLEVEL )) ; 
        } 
    
    
    }
    
object SENSITIVITYLEVEL_OnChange_1 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort OUTPUT = 0;
        
        RAMP_INFO TEMPRAMP;
        TEMPRAMP  = new RAMP_INFO();
        TEMPRAMP .PopulateDefaults();
        
        
        __context__.SourceCodeLine = 528;
        
        __context__.SourceCodeLine = 532;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SENSITIVITYLEVEL  .UshortValue == AUTOSETPOINTDATA.SENSITIVITY_LEVEL))  ) ) 
            {
            __context__.SourceCodeLine = 533;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 535;
        if ( Functions.TestForTrue  ( ( G_INITIALIZED)  ) ) 
            { 
            __context__.SourceCodeLine = 552;
            AUTOSETPOINTDATA . SENSITIVITY_LEVEL = (ushort) ( SENSITIVITYLEVEL  .UshortValue ) ; 
            __context__.SourceCodeLine = 553;
            CALCULATEXFERCONSTANTS (  __context__  ) ; 
            __context__.SourceCodeLine = 554;
            SAVEFILE (  __context__  ) ; 
            } 
        
        __context__.SourceCodeLine = 557;
        if ( Functions.TestForTrue  ( ( AUTOSETPOINTDATA.ENABLED)  ) ) 
            { 
            __context__.SourceCodeLine = 559;
            OUTPUT = (ushort) ( CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) ) ; 
            __context__.SourceCodeLine = 560;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( OUTPUT > AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ))  ) ) 
                {
                __context__.SourceCodeLine = 561;
                OUTPUT = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
                }
            
            __context__.SourceCodeLine = 562;
            SENDRAMP (  __context__ , (uint)( 50 ), (int)( OUTPUT )) ; 
            } 
        
        __context__.SourceCodeLine = 565;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CALIBRATE_NIGHT_OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 572;
        
        __context__.SourceCodeLine = 576;
        AUTOSETPOINTDATA . NIGHTTIME_SETPOINT = (ushort) ( SENSOR_INPUT  .UshortValue ) ; 
        __context__.SourceCodeLine = 577;
        AUTOSETPOINTDATA . NIGHTTIME_DIM_LEVEL = (int) ( DIM_LEVEL_OUT  .Value ) ; 
        __context__.SourceCodeLine = 578;
        NIGHTTIME_SETPOINT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_SETPOINT ) ; 
        __context__.SourceCodeLine = 579;
        NIGHTTIME_DIM_OUT  .Value = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
        __context__.SourceCodeLine = 581;
        CALCULATEXFERCONSTANTS (  __context__  ) ; 
        __context__.SourceCodeLine = 582;
        SAVEFILE (  __context__  ) ; 
        __context__.SourceCodeLine = 584;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MIN_DIM_LEVEL_CHANGED_OnRelease_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort OUTPUTLEVEL = 0;
        
        RAMP_INFO TEMPRAMP;
        TEMPRAMP  = new RAMP_INFO();
        TEMPRAMP .PopulateDefaults();
        
        
        __context__.SourceCodeLine = 594;
        
        __context__.SourceCodeLine = 598;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.MINDIMLEVEL == MIN_DIM_LEVEL_OUT  .Value))  ) ) 
            {
            __context__.SourceCodeLine = 599;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 601;
        AUTOSETPOINTDATA . MINDIMLEVEL = (int) ( MIN_DIM_LEVEL_OUT  .Value ) ; 
        __context__.SourceCodeLine = 602;
        SAVEFILE (  __context__  ) ; 
        __context__.SourceCodeLine = 603;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( AUTOSETPOINTDATA.ENABLED ) && Functions.TestForTrue ( G_INITIALIZED )) ))  ) ) 
            { 
            __context__.SourceCodeLine = 605;
            OUTPUTLEVEL = (ushort) ( CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) ) ; 
            __context__.SourceCodeLine = 606;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( OUTPUTLEVEL > AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ))  ) ) 
                {
                __context__.SourceCodeLine = 607;
                OUTPUTLEVEL = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
                }
            
            __context__.SourceCodeLine = 610;
            SENDRAMP (  __context__ , (uint)( 50 ), (int)( OUTPUTLEVEL )) ; 
            } 
        
        __context__.SourceCodeLine = 612;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SENSOR_INPUT_OnChange_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort OUTPUTLEVEL = 0;
        
        RAMP_INFO TEMPRAMP;
        TEMPRAMP  = new RAMP_INFO();
        TEMPRAMP .PopulateDefaults();
        
        
        __context__.SourceCodeLine = 622;
        
        __context__.SourceCodeLine = 626;
        if ( Functions.TestForTrue  ( ( G_BSENSORINPUTSEMAPHORE)  ) ) 
            {
            __context__.SourceCodeLine = 627;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 629;
        if ( Functions.TestForTrue  ( ( G_BDISABLESENSOR)  ) ) 
            {
            __context__.SourceCodeLine = 630;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 632;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (G_INITIALIZED == 0))  ) ) 
            { 
            __context__.SourceCodeLine = 634;
            INITIALIZE (  __context__  ) ; 
            __context__.SourceCodeLine = 635;
            G_INITIALIZED = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 636;
            return  this ; 
            } 
        
        __context__.SourceCodeLine = 639;
        G_BSENSORINPUTSEMAPHORE = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 641;
        if ( Functions.TestForTrue  ( ( AUTOSETPOINTDATA.ENABLED)  ) ) 
            { 
            __context__.SourceCodeLine = 643;
            OUTPUTLEVEL = (ushort) ( CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) ) ; 
            __context__.SourceCodeLine = 644;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( OUTPUTLEVEL > AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ))  ) ) 
                {
                __context__.SourceCodeLine = 645;
                OUTPUTLEVEL = (ushort) ( AUTOSETPOINTDATA.NIGHTTIME_DIM_LEVEL ) ; 
                }
            
            __context__.SourceCodeLine = 646;
            SENDRAMP (  __context__ , (uint)( RESPONSE_TIME  .Value ), (int)( OUTPUTLEVEL )) ; 
            } 
        
        __context__.SourceCodeLine = 649;
        G_BSENSORINPUTSEMAPHORE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 651;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESTARTPHOTOCELLWITHRESPONSETIME_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 658;
        
        __context__.SourceCodeLine = 664;
        ENABLEPHOTOCELL (  __context__ , (ushort)( 0 ), (uint)( RESPONSE_TIME  .Value )) ; 
        __context__.SourceCodeLine = 666;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object RESTART_PHOTOCELL_OnPush_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 673;
        
        __context__.SourceCodeLine = 678;
        ENABLEPHOTOCELL (  __context__ , (ushort)( 0 ), (uint)( RESTART_RAMP_TIME  .Value )) ; 
        __context__.SourceCodeLine = 680;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object DISABLE_OnPush_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 687;
        
        __context__.SourceCodeLine = 691;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.ENABLED == 0))  ) ) 
            {
            __context__.SourceCodeLine = 692;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 694;
        AUTOSETPOINTDATA . ENABLED = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 695;
        DISABLED  .Value = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 696;
        StopRamp ( DIM_LEVEL_OUT ) ; 
        __context__.SourceCodeLine = 700;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object DISABLEANDOFF_OnPush_8 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 707;
        
        __context__.SourceCodeLine = 711;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.ENABLED == 0))  ) ) 
            { 
            __context__.SourceCodeLine = 713;
            SENDRAMP (  __context__ , (uint)( OFFFADETIME  .Value ), (int)( 0 )) ; 
            } 
        
        else 
            { 
            __context__.SourceCodeLine = 717;
            AUTOSETPOINTDATA . ENABLED = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 718;
            DISABLED  .Value = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 719;
            SENDRAMP (  __context__ , (uint)( OFFFADETIME  .Value ), (int)( 0 )) ; 
            } 
        
        __context__.SourceCodeLine = 724;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SLOWER_RESPONSE_OnPush_9 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 731;
        
        __context__.SourceCodeLine = 735;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 200))  ) ) 
            { 
            __context__.SourceCodeLine = 737;
            AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 500 ) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 739;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 500))  ) ) 
                { 
                __context__.SourceCodeLine = 741;
                AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 1000 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 743;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 1000))  ) ) 
                    { 
                    __context__.SourceCodeLine = 745;
                    AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 3000 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 747;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 3000))  ) ) 
                        { 
                        __context__.SourceCodeLine = 749;
                        AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 6000 ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 751;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 6000))  ) ) 
                            { 
                            __context__.SourceCodeLine = 753;
                            AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 30000 ) ; 
                            } 
                        
                        }
                    
                    }
                
                }
            
            }
        
        __context__.SourceCodeLine = 756;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (RESPONSE_TIME  .Value == AUTOSETPOINTDATA.RESPONSE_TIME))  ) ) 
            {
            __context__.SourceCodeLine = 757;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 759;
        RESPONSE_TIME  .Value = (ushort) ( AUTOSETPOINTDATA.RESPONSE_TIME ) ; 
        __context__.SourceCodeLine = 760;
        SAVEFILE (  __context__  ) ; 
        __context__.SourceCodeLine = 762;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object FASTER_RESPONSE_OnPush_10 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 769;
        
        __context__.SourceCodeLine = 773;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 30000))  ) ) 
            { 
            __context__.SourceCodeLine = 775;
            AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 6000 ) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 777;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 6000))  ) ) 
                { 
                __context__.SourceCodeLine = 779;
                AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 3000 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 781;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 3000))  ) ) 
                    { 
                    __context__.SourceCodeLine = 783;
                    AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 1000 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 785;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 1000))  ) ) 
                        { 
                        __context__.SourceCodeLine = 787;
                        AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 500 ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 789;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (AUTOSETPOINTDATA.RESPONSE_TIME == 500))  ) ) 
                            { 
                            __context__.SourceCodeLine = 791;
                            AUTOSETPOINTDATA . RESPONSE_TIME = (uint) ( 200 ) ; 
                            } 
                        
                        }
                    
                    }
                
                }
            
            }
        
        __context__.SourceCodeLine = 794;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (RESPONSE_TIME  .Value == AUTOSETPOINTDATA.RESPONSE_TIME))  ) ) 
            {
            __context__.SourceCodeLine = 795;
            return  this ; 
            }
        
        __context__.SourceCodeLine = 797;
        RESPONSE_TIME  .Value = (ushort) ( AUTOSETPOINTDATA.RESPONSE_TIME ) ; 
        __context__.SourceCodeLine = 798;
        SAVEFILE (  __context__  ) ; 
        __context__.SourceCodeLine = 800;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object TEMPRAISE_OnRelease_11 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 808;
        
        __context__.SourceCodeLine = 812;
        G_OUTPUTVALUE = (ushort) ( CALCULATEOUTPUT( __context__ , (ushort)( SENSOR_INPUT  .UshortValue ) ) ) ; 
        __context__.SourceCodeLine = 813;
        G_LOFFSET = (int) ( (DIM_LEVEL_OUT  .Value - G_OUTPUTVALUE) ) ; 
        __context__.SourceCodeLine = 814;
        G_BDISABLESENSOR = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 815;
        RESPONSE_TIME  .Value = (ushort) ( AUTOSETPOINTDATA.RESPONSE_TIME ) ; 
        __context__.SourceCodeLine = 816;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (G_LOFFSET != AUTOSETPOINTDATA.OFFSET))  ) ) 
            { 
            __context__.SourceCodeLine = 818;
            SAVEFILE (  __context__  ) ; 
            } 
        
        __context__.SourceCodeLine = 821;
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object TEMPRAISE_OnPush_12 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 828;
        
        __context__.SourceCodeLine = 832;
        G_BDISABLESENSOR = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 833;
        RESPONSE_TIME  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 834;
        CancelAllWait ( ) ; 
        __context__.SourceCodeLine = 836;
        
        
        
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
        
        __context__.SourceCodeLine = 845;
        G_BDISABLESENSOR = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 846;
        G_BSENSORINPUTSEMAPHORE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 847;
        G_PREVIOUSTARGET = (int) ( 0 ) ; 
        __context__.SourceCodeLine = 848;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 850;
        G_INITIALIZED = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 851;
        G_FILEPATH  .UpdateValue ( FILELOCATION + "Photocell_" + Functions.ItoA (  (int) ( ID  .Value ) ) + ".dat"  ) ; 
        __context__.SourceCodeLine = 852;
        G_LOFFSET = (int) ( 0 ) ; 
        __context__.SourceCodeLine = 853;
        G_SEMAPHORE = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 854;
        LOADFILE (  __context__  ) ; 
        __context__.SourceCodeLine = 855;
        G_BDISABLESENSOR = (ushort) ( 0 ) ; 
        
        
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
    G_FILEPATH  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 50, this );
    AUTOSETPOINTDATA  = new AUTOLEVEL( this, true );
    AUTOSETPOINTDATA .PopulateCustomAttributeList( false );
    
    DISABLE = new Crestron.Logos.SplusObjects.DigitalInput( DISABLE__DigitalInput__, this );
    m_DigitalInputList.Add( DISABLE__DigitalInput__, DISABLE );
    
    DISABLEANDOFF = new Crestron.Logos.SplusObjects.DigitalInput( DISABLEANDOFF__DigitalInput__, this );
    m_DigitalInputList.Add( DISABLEANDOFF__DigitalInput__, DISABLEANDOFF );
    
    RESTART_PHOTOCELL = new Crestron.Logos.SplusObjects.DigitalInput( RESTART_PHOTOCELL__DigitalInput__, this );
    m_DigitalInputList.Add( RESTART_PHOTOCELL__DigitalInput__, RESTART_PHOTOCELL );
    
    RESTARTPHOTOCELLWITHRESPONSETIME = new Crestron.Logos.SplusObjects.DigitalInput( RESTARTPHOTOCELLWITHRESPONSETIME__DigitalInput__, this );
    m_DigitalInputList.Add( RESTARTPHOTOCELLWITHRESPONSETIME__DigitalInput__, RESTARTPHOTOCELLWITHRESPONSETIME );
    
    CALIBRATE_NIGHT = new Crestron.Logos.SplusObjects.DigitalInput( CALIBRATE_NIGHT__DigitalInput__, this );
    m_DigitalInputList.Add( CALIBRATE_NIGHT__DigitalInput__, CALIBRATE_NIGHT );
    
    FASTER_RESPONSE = new Crestron.Logos.SplusObjects.DigitalInput( FASTER_RESPONSE__DigitalInput__, this );
    m_DigitalInputList.Add( FASTER_RESPONSE__DigitalInput__, FASTER_RESPONSE );
    
    SLOWER_RESPONSE = new Crestron.Logos.SplusObjects.DigitalInput( SLOWER_RESPONSE__DigitalInput__, this );
    m_DigitalInputList.Add( SLOWER_RESPONSE__DigitalInput__, SLOWER_RESPONSE );
    
    DIMLEVELMINCHANGED = new Crestron.Logos.SplusObjects.DigitalInput( DIMLEVELMINCHANGED__DigitalInput__, this );
    m_DigitalInputList.Add( DIMLEVELMINCHANGED__DigitalInput__, DIMLEVELMINCHANGED );
    
    MIN_DIM_LEVEL_CHANGED = new Crestron.Logos.SplusObjects.DigitalInput( MIN_DIM_LEVEL_CHANGED__DigitalInput__, this );
    m_DigitalInputList.Add( MIN_DIM_LEVEL_CHANGED__DigitalInput__, MIN_DIM_LEVEL_CHANGED );
    
    TEMPRAISE = new Crestron.Logos.SplusObjects.DigitalInput( TEMPRAISE__DigitalInput__, this );
    m_DigitalInputList.Add( TEMPRAISE__DigitalInput__, TEMPRAISE );
    
    TEMPLOWER = new Crestron.Logos.SplusObjects.DigitalInput( TEMPLOWER__DigitalInput__, this );
    m_DigitalInputList.Add( TEMPLOWER__DigitalInput__, TEMPLOWER );
    
    DISABLED = new Crestron.Logos.SplusObjects.DigitalOutput( DISABLED__DigitalOutput__, this );
    m_DigitalOutputList.Add( DISABLED__DigitalOutput__, DISABLED );
    
    SEND_RAMP_1 = new Crestron.Logos.SplusObjects.DigitalOutput( SEND_RAMP_1__DigitalOutput__, this );
    m_DigitalOutputList.Add( SEND_RAMP_1__DigitalOutput__, SEND_RAMP_1 );
    
    SEND_RAMP_2 = new Crestron.Logos.SplusObjects.DigitalOutput( SEND_RAMP_2__DigitalOutput__, this );
    m_DigitalOutputList.Add( SEND_RAMP_2__DigitalOutput__, SEND_RAMP_2 );
    
    DALI_OFF = new Crestron.Logos.SplusObjects.DigitalOutput( DALI_OFF__DigitalOutput__, this );
    m_DigitalOutputList.Add( DALI_OFF__DigitalOutput__, DALI_OFF );
    
    SENSITIVITYLEVEL = new Crestron.Logos.SplusObjects.AnalogInput( SENSITIVITYLEVEL__AnalogSerialInput__, this );
    m_AnalogInputList.Add( SENSITIVITYLEVEL__AnalogSerialInput__, SENSITIVITYLEVEL );
    
    SENSOR_INPUT = new Crestron.Logos.SplusObjects.AnalogInput( SENSOR_INPUT__AnalogSerialInput__, this );
    m_AnalogInputList.Add( SENSOR_INPUT__AnalogSerialInput__, SENSOR_INPUT );
    
    DIM_LEVEL_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( DIM_LEVEL_OUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( DIM_LEVEL_OUT__AnalogSerialOutput__, DIM_LEVEL_OUT );
    
    TARGET_DIM_LEVEL = new Crestron.Logos.SplusObjects.AnalogOutput( TARGET_DIM_LEVEL__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( TARGET_DIM_LEVEL__AnalogSerialOutput__, TARGET_DIM_LEVEL );
    
    DIM_FADE_TIME = new Crestron.Logos.SplusObjects.AnalogOutput( DIM_FADE_TIME__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( DIM_FADE_TIME__AnalogSerialOutput__, DIM_FADE_TIME );
    
    NIGHTTIME_SETPOINT = new Crestron.Logos.SplusObjects.AnalogOutput( NIGHTTIME_SETPOINT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( NIGHTTIME_SETPOINT__AnalogSerialOutput__, NIGHTTIME_SETPOINT );
    
    NIGHTTIME_DIM_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( NIGHTTIME_DIM_OUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( NIGHTTIME_DIM_OUT__AnalogSerialOutput__, NIGHTTIME_DIM_OUT );
    
    RESPONSE_TIME = new Crestron.Logos.SplusObjects.AnalogOutput( RESPONSE_TIME__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( RESPONSE_TIME__AnalogSerialOutput__, RESPONSE_TIME );
    
    SENSITIVITYLEVELOUT = new Crestron.Logos.SplusObjects.AnalogOutput( SENSITIVITYLEVELOUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( SENSITIVITYLEVELOUT__AnalogSerialOutput__, SENSITIVITYLEVELOUT );
    
    MIN_DIM_LEVEL_OUT = new Crestron.Logos.SplusObjects.AnalogOutput( MIN_DIM_LEVEL_OUT__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( MIN_DIM_LEVEL_OUT__AnalogSerialOutput__, MIN_DIM_LEVEL_OUT );
    
    DIMOUTPUTMINCHANGE = new Crestron.Logos.SplusObjects.AnalogOutput( DIMOUTPUTMINCHANGE__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( DIMOUTPUTMINCHANGE__AnalogSerialOutput__, DIMOUTPUTMINCHANGE );
    
    RESTART_RAMP_TIME = new UShortParameter( RESTART_RAMP_TIME__Parameter__, this );
    m_ParameterList.Add( RESTART_RAMP_TIME__Parameter__, RESTART_RAMP_TIME );
    
    NIGHTTIME_SETPOINT_DEFAULT = new UShortParameter( NIGHTTIME_SETPOINT_DEFAULT__Parameter__, this );
    m_ParameterList.Add( NIGHTTIME_SETPOINT_DEFAULT__Parameter__, NIGHTTIME_SETPOINT_DEFAULT );
    
    OFFFADETIME = new UShortParameter( OFFFADETIME__Parameter__, this );
    m_ParameterList.Add( OFFFADETIME__Parameter__, OFFFADETIME );
    
    ID = new UShortParameter( ID__Parameter__, this );
    m_ParameterList.Add( ID__Parameter__, ID );
    
    FILELOCATION = new StringParameter( FILELOCATION__Parameter__, this );
    m_ParameterList.Add( FILELOCATION__Parameter__, FILELOCATION );
    
    SAVEWAIT_Callback = new WaitFunction( SAVEWAIT_CallbackFn );
    
    DIMLEVELMINCHANGED.OnDigitalPush.Add( new InputChangeHandlerWrapper( DIMLEVELMINCHANGED_OnPush_0, false ) );
    SENSITIVITYLEVEL.OnAnalogChange.Add( new InputChangeHandlerWrapper( SENSITIVITYLEVEL_OnChange_1, false ) );
    CALIBRATE_NIGHT.OnDigitalChange.Add( new InputChangeHandlerWrapper( CALIBRATE_NIGHT_OnChange_2, false ) );
    MIN_DIM_LEVEL_CHANGED.OnDigitalRelease.Add( new InputChangeHandlerWrapper( MIN_DIM_LEVEL_CHANGED_OnRelease_3, false ) );
    SENSOR_INPUT.OnAnalogChange.Add( new InputChangeHandlerWrapper( SENSOR_INPUT_OnChange_4, false ) );
    RESTARTPHOTOCELLWITHRESPONSETIME.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESTARTPHOTOCELLWITHRESPONSETIME_OnPush_5, false ) );
    RESTART_PHOTOCELL.OnDigitalPush.Add( new InputChangeHandlerWrapper( RESTART_PHOTOCELL_OnPush_6, false ) );
    DISABLE.OnDigitalPush.Add( new InputChangeHandlerWrapper( DISABLE_OnPush_7, false ) );
    DISABLEANDOFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( DISABLEANDOFF_OnPush_8, false ) );
    SLOWER_RESPONSE.OnDigitalPush.Add( new InputChangeHandlerWrapper( SLOWER_RESPONSE_OnPush_9, false ) );
    FASTER_RESPONSE.OnDigitalPush.Add( new InputChangeHandlerWrapper( FASTER_RESPONSE_OnPush_10, false ) );
    TEMPRAISE.OnDigitalRelease.Add( new InputChangeHandlerWrapper( TEMPRAISE_OnRelease_11, false ) );
    TEMPLOWER.OnDigitalRelease.Add( new InputChangeHandlerWrapper( TEMPRAISE_OnRelease_11, false ) );
    TEMPRAISE.OnDigitalPush.Add( new InputChangeHandlerWrapper( TEMPRAISE_OnPush_12, false ) );
    TEMPLOWER.OnDigitalPush.Add( new InputChangeHandlerWrapper( TEMPRAISE_OnPush_12, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public CrestronModuleClass_PHOTOCELL_PROPORTIONAL_CONTROL_V1_7 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction SAVEWAIT_Callback;


const uint DISABLE__DigitalInput__ = 0;
const uint DISABLEANDOFF__DigitalInput__ = 1;
const uint RESTART_PHOTOCELL__DigitalInput__ = 2;
const uint RESTARTPHOTOCELLWITHRESPONSETIME__DigitalInput__ = 3;
const uint CALIBRATE_NIGHT__DigitalInput__ = 4;
const uint FASTER_RESPONSE__DigitalInput__ = 5;
const uint SLOWER_RESPONSE__DigitalInput__ = 6;
const uint DIMLEVELMINCHANGED__DigitalInput__ = 7;
const uint SENSITIVITYLEVEL__AnalogSerialInput__ = 0;
const uint MIN_DIM_LEVEL_CHANGED__DigitalInput__ = 8;
const uint TEMPRAISE__DigitalInput__ = 9;
const uint TEMPLOWER__DigitalInput__ = 10;
const uint SENSOR_INPUT__AnalogSerialInput__ = 1;
const uint DISABLED__DigitalOutput__ = 0;
const uint SEND_RAMP_1__DigitalOutput__ = 1;
const uint SEND_RAMP_2__DigitalOutput__ = 2;
const uint DALI_OFF__DigitalOutput__ = 3;
const uint DIM_LEVEL_OUT__AnalogSerialOutput__ = 0;
const uint TARGET_DIM_LEVEL__AnalogSerialOutput__ = 1;
const uint DIM_FADE_TIME__AnalogSerialOutput__ = 2;
const uint NIGHTTIME_SETPOINT__AnalogSerialOutput__ = 3;
const uint NIGHTTIME_DIM_OUT__AnalogSerialOutput__ = 4;
const uint RESPONSE_TIME__AnalogSerialOutput__ = 5;
const uint SENSITIVITYLEVELOUT__AnalogSerialOutput__ = 6;
const uint MIN_DIM_LEVEL_OUT__AnalogSerialOutput__ = 7;
const uint DIMOUTPUTMINCHANGE__AnalogSerialOutput__ = 8;
const uint RESTART_RAMP_TIME__Parameter__ = 10;
const uint NIGHTTIME_SETPOINT_DEFAULT__Parameter__ = 11;
const uint OFFFADETIME__Parameter__ = 12;
const uint FILELOCATION__Parameter__ = 13;
const uint ID__Parameter__ = 14;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
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

[SplusStructAttribute(-1, true, false)]
public class AUTOLEVEL : SplusStructureBase
{

    [SplusStructAttribute(0, false, false)]
    public ushort  AUTO_X = 0;
    
    [SplusStructAttribute(1, false, false)]
    public ushort  AUTO_Y = 0;
    
    [SplusStructAttribute(2, false, false)]
    public ushort  SENSITIVITY_LEVEL = 0;
    
    [SplusStructAttribute(3, false, false)]
    public ushort  NIGHTTIME_SETPOINT = 0;
    
    [SplusStructAttribute(4, false, false)]
    public int  NIGHTTIME_DIM_LEVEL = 0;
    
    [SplusStructAttribute(5, false, false)]
    public int  MINDIMLEVEL = 0;
    
    [SplusStructAttribute(6, false, false)]
    public int  OFFSET = 0;
    
    [SplusStructAttribute(7, false, false)]
    public uint  RESPONSE_TIME = 0;
    
    [SplusStructAttribute(8, false, false)]
    public uint  SLOPEVALUE = 0;
    
    [SplusStructAttribute(9, false, false)]
    public uint  YINTERCEPT = 0;
    
    [SplusStructAttribute(10, false, false)]
    public ushort  ENABLED = 0;
    
    [SplusStructAttribute(11, false, false)]
    public ushort  DIMLEVELMINCHANGE = 0;
    
    
    public AUTOLEVEL( SplusObject __caller__, bool bIsStructureVolatile ) : base ( __caller__, bIsStructureVolatile )
    {
        
        
    }
    
}

}
