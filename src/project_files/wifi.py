import network, time, pycom

pycom.heartbeat(False)
wlan = network.WLAN(mode=network.WLAN.STA)

# Connecting  to wifi
def connect_wifi():
    wlan.connect('Ubantu', auth=(network.WLAN.WPA2, 'ubantuwlak'))
    while not wlan.isconnected():
        time.sleep_ms(50)
    if not wlan.isconnected():
        pycom.rgbled(0xff0000)
    else:
        pycom.rgbled(0x00ff00)
