//#include <PID.h>
#include <16F877A.h>
#device ADC=10

#FUSES NOWDT                    //No Watch Dog Timer
#FUSES NOBROWNOUT               //No brownout reset
#FUSES NOLVP                    //No low voltage prgming, B3(PIC16) or B5(PIC18) used for I/O

#use delay(crystal=20000000)
#use rs232(baud=9600,parity=E,xmit=PIN_C6,rcv=PIN_C7,bits=8,stream=PORT1)
#use i2c(Master,Fast,sda=PIN_C4,scl=PIN_C5)

 
#include <stdlib.h>

#include "LCD_I2C.c"
#include "sensor_temperatura.c"

#define LCD_ADDR 0x4E

//PIN_B0 -> Detecta passagem por 0
//PIN_B1 -> Sensor de temperatura
//PIN_B2 -> Ativa a resistencia


//Temperatura
float y = 35.0f; //   Temperatura atual
float r = 35.0f; //   setPoint

float u_atual = 0;
float u_anterior = 0;
float e_atual = 0;
float e_anterior = 0;
const float Kp = 1/0.9;
const float Ti = 10.33f;
const float T  = 1;


//Timers
int overflowCount = 0;
int1 umSegundo = FALSE;
int1 meioSegundo = FALSE;
int1 umQuartoSegundo = FALSE;
int1 umOitavoSegundo = FALSE;

//Action
int ticks = 0;

//Comunicacao
#define BUFF_LEN 30 
char string[BUFF_LEN]; 
int1 have_string=FALSE; 


#INT_RTCC
void  RTCC_isr(void) 
{
   overflowCount++;
   
   if (overflowCount % 9 == 0)
      umOitavoSegundo = TRUE;   
      
   if (overflowCount % 19 == 0)
      umQuartoSegundo = TRUE;
   
   if (overflowCount % 38 == 0)
      meioSegundo = TRUE;
      
   if (overflowCount % 76 == 0)
      umSegundo = TRUE;
      
     
   if (overflowCount >= 76) overflowCount = 0;//Por seguranca
}

#INT_EXT
void  EXT_isr(void) 
{
   ticks++;
   
   if (u_atual > ticks)
      output_high(PIN_B2);
   else
      output_low(PIN_B2);
   
   if (ticks > 60)
      ticks = 0;
}

#INT_RDA
void  RDA_isr(void) 
{
    static int ctr=0; 
    int ch; 
    ch=getc(); 
    if (ch=='\n') 
    { 
        string[ctr]='\0'; 
        ctr=0; 
        have_string=TRUE; 
        return; 
    } 
    string[ctr++]=ch; 
    if (ctr==BUFF_LEN) 
        ctr--; //throw away characters if buffer overflows 
}

void resetTimes()
{
   disable_interrupts(INT_RTCC);
   
   umSegundo = FALSE;
   meioSegundo = FALSE;
   umQuartoSegundo = FALSE;
   umOitavoSegundo = FALSE;  
   
   enable_interrupts(INT_RTCC);
}

void main()
{
   setup_adc_ports(AN0);
   setup_adc(ADC_CLOCK_DIV_2);
   setup_timer_0(RTCC_INTERNAL|RTCC_DIV_256|RTCC_8_bit);      //13.1 ms overflow
   
   enable_interrupts(INT_RTCC);
   enable_interrupts(INT_EXT);
   enable_interrupts(INT_RDA);
   enable_interrupts(GLOBAL);
   
   y = ds1820_read();
   delay_ms(1000);
   initializeLCD(LCD_ADDR, 16, 2);

   while(TRUE)
   {
       if (umSegundo)
       { 
            y = ds1820_read();   
            delay_ms(15);
       }
   
       if (umOitavoSegundo) 
       { 
            if (have_string)
            {
               have_string=FALSE; 
               //setCursor(0, 1);
               r = atof(string);  
               //printfloat(r);
            }   
            else
            {           
                if (umOitavoSegundo)
                {
                    fprintf(PORT1, "t%lf\n", y);
                    fprintf(PORT1, "u%lf\n", u_atual);
                }
            }
       }
       
       if (umSegundo)
       {
            e_anterior = e_atual;
            e_atual = r - y; //Erro == resposta - setPoint
       
            u_anterior = u_atual;
            u_atual = u_anterior + Kp*e_atual - Kp*(1-T/Ti)*e_anterior; //Saida do PID(PI)
            
            if (u_atual > 60) u_atual = 60.0f;
            if (u_atual < 00) u_atual = 0.0f;
       }
       
       if (meioSegundo)
       {
         static char st[]="S.P.: ";
         static char ta[]="T.A.: ";
         home();
         clear();
         printstr(ta);
         printfloat(y);
         setCursor(0, 1);
         printstr(st);
         printfloat(r);
       }
       //setCursor(0, 1);
      // printfloat(u_atual);
       
       resetTimes();
       //resetTimes();
   }

}
