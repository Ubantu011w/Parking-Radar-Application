import time
from machine import Timer
from machine import Pin,PWM
from mqtt import MQTTClient
import network
import pycom
from wifi import *

adafruit_connected=False
pycom.heartbeat(False)

# Connecting to Mqtt
def adafruit_connect():
    global client, adafruit_connected
    client = MQTTClient("Charbel", "io.adafruit.com",user="ubantu011",password="aio_btUr85N6wp5RGRCgGfnkV8poj1pr", port=1883)
    client.connect()
    adafruit_connected=True

# Getting distance from a certain sensor
def get_distance(echo,trigger):
    chrono = Timer.Chrono()     # Chrono() räknar time spans.

    chrono.reset()

    trigger(1)
    time.sleep_us(10)
    trigger(0)

    while echo() == 0:
        pass

    chrono.start()

    while echo() == 1:
        pass

    chrono.stop()
    distance = (chrono.read_us() * .034) /2 # (Time(us) * .034) / 2, ljudhastighet / 2
    return str(int(distance)) + ";"     # Returning a string so it can be added up to one variable "string"
                                        # and send it to Adafruit.


# Initialise Ultrasonic sensors pins
echo = Pin('P11', mode=Pin.IN)
trigger = Pin('P21', mode=Pin.OUT)

echo2 = Pin('P10', mode=Pin.IN)
trigger2= Pin('P20', mode=Pin.OUT)

echo3 = Pin('P9', mode=Pin.IN)
trigger3= Pin('P22', mode=Pin.OUT)

connect_wifi()
adafruit_connect()

trigger(0)
trigger2(0)
trigger3(0)


while True:
    # Checking connection.
    if not wlan.isconnected():
        connect_wifi()
    elif adafruit_connected==False:
        adafruit_connect()

    distance = get_distance(echo,trigger)
    distance2 = get_distance(echo2,trigger2)
    distance3 = get_distance(echo3,trigger3)

    string = distance + distance2 + distance3        # Gathering all data into one string.

    client.publish(topic="ubantu011/feeds/parking", msg=string) # Sending the string.

    time.sleep(1)                   # We wait a whole second because data points to
                                    #adafruit are limited by 60 datapoints per minute.
