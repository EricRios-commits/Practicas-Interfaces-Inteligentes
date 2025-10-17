# Práctica 4

1. Crea una escena con 5 esferas, rojas que las etiquetas de tipo 1, y verdes de tipo 2. Añade un cubo y un cilindro. Crea la siguiente mecánica: cuando el cubo colisiona con el cilindro, las esferas de tipo 1 se dirigen hacia una de las esferas de tipo 2 que fijes de antemano y las esferas de tipo 2 se desplazan hacia el cilindro.
![](ejercicio1.gif)

2. Sustituye los objetos geométricos por humanoides  que encontrarás en el enlace y en este enlace 
![](ejercicio2.gif)

3. Adapta la escena anterior para que existan humanoides de tipo 1 y de tipo 2, así como diferentes tipos de escudos, tipo 1 y tipo 2:
    - Cuando el cubo colisiona con cualquier humanoide  tipo 2,  los del grupo 1 se acercan a un escudo seleccionado. Cuando el cubo toca cualquier humanoide del grupo 1 se dirigen hacia los escudos del grupo 2 que serán objetos físicos. Si algún humanoide a colisiona con uno de ellos debe cambiar de color. 
    - Cubo colisiona con tipo 2:
        ![](ej3-touched-type2.gif)
    - Cubo colisiona con tipo 1: 
        ![](ej3-touched-type1.gif)

4. Cuando el cubo se aproxima al objeto de referencia, los humanoides del grupo 1 se teletransportan a un escudo objetivo que debes fijar de antemano.Los humanoides del grupo 2 se orientan hacia un objeto ubicado en la escena con ese propósito. 
![](ej4.gif)

5. Implementar la mecánica de recolectar escudos en la escena que actualicen la puntuación del jugador. Los escudos de tipo 1 suman 5 puntos y los de tipo 2 suman 10. Mostrar la puntuación en la consola.
![](ej5.gif)

6. Partiendo del script anterior crea una interfaz que muestre la puntuación que va obteniendo el cubo. 
![](ej6.gif)

7. Partiendo de los ejercicios anteriores, implementa una mecánica en la que cada 100 puntos el jugador obtenga una recompensa que se muestre en la UI.
![](ej7.gif)

8. Genera una escena que incluya elementos que se ajusten a la escena del prototipo y alguna de las mecánicas anteriores.


9. Implementa el ejercicio 3 siendo el cubo un objeto físico.
    Esto se realizó ya previamente para agilizar la visualización de los efectos del cubo (objeto físico controlable). 
    ![](ej3-touched-type1.gif)