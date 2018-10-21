
#define LAST_DIGITAL_PIN 12
#define FIRST_DIGITAL_PIN  2

void setup() {
  for (int i = FIRST_DIGITAL_PIN; i <= LAST_DIGITAL_PIN; i++)
  { 
      pinMode(i, INPUT);   
  }      
  pinMode(13, OUTPUT);
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(A0, INPUT); 
  digitalWrite(13, HIGH);
  
}

void loop() {
  bool check_for_short_circuit = false;
  for (int i = FIRST_DIGITAL_PIN; i <= LAST_DIGITAL_PIN; i++)
   {
      if (digitalRead(i) == HIGH)
      {
        check_for_short_circuit = true;
        break;
      }
   }
   if (check_for_short_circuit == true)
   {
      digitalWrite(LED_BUILTIN, HIGH);
   }
   else
   {
      digitalWrite(LED_BUILTIN, LOW);
   }
}
