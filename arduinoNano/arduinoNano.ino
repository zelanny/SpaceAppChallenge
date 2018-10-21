#include "LinkedList.h";
#define LAST_DIGITAL_PIN 12
#define FIRST_DIGITAL_PIN  2

typedef struct Record
{
  char axis;
  int coordinate;
};
  
void setup() {
  Serial.begin(9600);
  for (int i = FIRST_DIGITAL_PIN; i <= LAST_DIGITAL_PIN; i++)
  { 
      pinMode(i, INPUT);   
  }      
  pinMode(13, OUTPUT);
  pinMode(A0, INPUT); 
}

void loop() {
   LinkedList<Record> result = LinkedList<Record>();
   digitalWrite(13, HIGH);
   for (int i = FIRST_DIGITAL_PIN; i <= LAST_DIGITAL_PIN; i++)
   {
      if (digitalRead(i) == LOW)
      {
          Record failedPin;
          
          if (i % 2 == 0)
          { 
              failedPin.axis = 't';
              failedPin.coordinate = i / 2; // Top contacts for x-axis
          }
          else
          {
              failedPin.axis = 'l';
              failedPin.coordinate = (i - 1) / 2; // Left contacts for y-axis
          }

          result.add(failedPin);
      }
   }
   if (digitalRead(A0) == LOW)
      {
          Record failedPin;
          failedPin.axis = 'l';
          failedPin.coordinate = 6; // Top contacts for x-axis
          result.add(failedPin);
      }
   Serial.write((const byte *)&result, sizeof(result));
   
   result.clear();
   digitalWrite(13, LOW);
   delay(1000);
}
