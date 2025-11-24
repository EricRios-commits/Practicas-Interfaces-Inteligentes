Experimentación simple con micrófono y webcam en Unity.

1. Utilizar la escena de los guerreros y activa la reproduccion de alguno de los
sonidos incluidos en la carpeta adjunta cuando un guerrero alcanza algun
objetivo. Para reproducir sonido en una aplicación Unity es necesario utilizar
un objeto AudioSource. El objeto AudioSource reproduce el sonido que
contiene un AudioClip, que podemos instanciar arrastrando desde el editor
el asset con el clip de audio que esté importado en la escena.
![]


2. Del mismo modo que podemos reproducir un sonido previamente grabado, podemos hacer que se reproduzaca el AudioClip que genera el micrófono utilizando la función Microphone.Start. Crea una escena en la que estés en un espacio abierto en el que habrá una pantalla central con altavoces que al pulsar la tecla R reproduzcan el sonido que se obtenga por el micrófono del dispositivo.

3. En este caso, debes utilizar lo que capta la cámara para reproducirlo en una
pantalla ubicada en la escena anterior. Para ver las imágenes grabadas debe
asociarse el objeto WebCamTexture al atributo main Texture de un material y
seguidamente llamar a la función Play() de dicho objeto. Por tanto, a través
del objeto Renderer del elemento que uses como pantalla accedes a la
propiedad material y a su mainTexture asignarle lo que se capta por la
cámara. Para pausar la reproducción basta llamar a la función Pause() y,
para detenerla, a la función Stop().