#include <EEPROM.h>

int soil = A0;
int relay = 8;
bool isProcessRunning = true;
bool isCommunicationEnabled = true; // Flag to control communication
int storedHumidityValueAddress = 0;
int humidityThreshold = 0; // Variable to store the humidity threshold

unsigned long lastLoopTime = 0;
unsigned long loopInterval = 1000; // Set the loop interval in milliseconds
char lastState = 'A';

void setup() {
  isCommunicationEnabled = true;
  pinMode(soil, INPUT);
  pinMode(relay, OUTPUT);
  digitalWrite(relay, HIGH); // Relay initially HIGH (off)
  Serial.begin(9600);

  // Read the humidity threshold from EEPROM
  EEPROM.get(storedHumidityValueAddress, humidityThreshold);
  if (isProcessRunning) {
    Serial.println("Arduino is in Automatic Mode");
  } else {
    Serial.println("Arduino is in Manual Mode");
  }
}

void loop() {
  unsigned long currentMillis = millis();
  char command; // Declare command here



  // Automatic mode
  if (currentMillis - lastLoopTime >= loopInterval) {
    lastLoopTime = currentMillis;

    // Automatic mode
    if (isProcessRunning) {
      int moistureLevel = analogRead(soil);
      Serial.print("Soil Moisture Reading: ");
      Serial.println(moistureLevel);
      int dryValue = 1022;
      int wetValue = 400;

      // Compare moistureLevel with the stored humidity threshold
      if (moistureLevel > humidityThreshold) {
        digitalWrite(relay, LOW); // Watering needed
      } else {
        digitalWrite(relay, HIGH); // Soil is moist enough
      }
    }



    // Manual mode
    if (!isProcessRunning && isCommunicationEnabled) {
      // Manual mode logic
      if (command == 'P') {
        Serial.println("Pump ON");
        digitalWrite(relay, LOW); // Turn on the relay in manual mode (Pump ON)
      } else if (command == 'S') {
        Serial.println("Pump OFF");
        digitalWrite(relay, HIGH); // Turn off the relay in manual mode (Pump OFF)
      }
      // Add more manual mode commands as needed
    }
  }

  while (Serial.available() > 0) {
    command = Serial.read();
      if (command == 'M' || command == 'A')
    lastState = command;


    // Toggle between manual and automatic modes
    if (command == 'M') {
      isProcessRunning = false;
      isCommunicationEnabled = true; // Enable communication when switching to manual mode
      //Serial.println("Switched to Manual Mode");
      digitalWrite(relay, HIGH); // Turn off the relay when switching to manual mode
      Serial.println("Arduino is in Manual Mode");
    } else if (command == 'A') {
      isProcessRunning = true;
      Serial.println("Arduino is in Automatic Mode");
    } else if (command == 'P' && !isProcessRunning && isCommunicationEnabled) {
      Serial.println("Pump ON");
      digitalWrite(relay, LOW); // Turn on the relay in manual mode (Pump ON)
    } else if (command == 'S' && !isProcessRunning && isCommunicationEnabled) {
      Serial.println("Pump OFF");
      digitalWrite(relay, HIGH); // Turn off the relay in manual mode (Pump OFF)
    } else if (command == 'O') {
      if (lastState == 'M') {
        isProcessRunning = false;
      }
      if (lastState == 'A') {
        isProcessRunning = true;
      }
      isCommunicationEnabled = true; // Enable communication when starting the process
      Serial.println("Process ON");
      //isProcessRunning = true;
      // Additional actions if needed when starting the process
    } else if (command == 'F' && isCommunicationEnabled) {
      isCommunicationEnabled = false; // Disable communication when stopping the process
      Serial.println("Process OFF");
      digitalWrite(relay, HIGH);
      isProcessRunning = false;
    } else if (command == 'T' && isCommunicationEnabled) {
      // Read the next characters until a newline is received
      char humidityValueStr[10]; // Assuming the humidity value won't exceed 10 characters
      Serial.readBytesUntil('\n', humidityValueStr, sizeof(humidityValueStr));

      // Extract the numeric part after 'T'
      int receivedHumidityValue = atoi(humidityValueStr);

      // Use the received humidity value as needed
      Serial.print("Received Humidity Value: ");
      Serial.println(receivedHumidityValue);

      // Store the humidity value in EEPROM
      EEPROM.put(storedHumidityValueAddress, receivedHumidityValue);

      // Update the humidity threshold variable
      humidityThreshold = receivedHumidityValue;
    }
  }
}
