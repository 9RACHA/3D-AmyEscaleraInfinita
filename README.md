# 3D-AmyEscaleraInfinita

## OnTriggerEnter:
Es un método que se utiliza en Unity para detectar cuando un objeto colisiona con un trigger. Un trigger es un colisionador con la propiedad "Is Trigger" habilitada, que permite que otros objetos atraviesen su área sin generar una colisión física, pero aún así se puede detectar cuando otro objeto entra en su volumen. 

Realizar un videojuego llamado AmyYLaEscaleraInfinita. En el juego el personaje de Amy tiene que tomar cosas(things) de unos estantes y llevarlas por una plataforma descendente hasta un teletransportador, donde las cosas desaparecen de nuestro universo. Luego, Amy continua su camino para, descendiendo de nuevo por otra plataforma, volver al lugar de partida.
Se suministra un proyecto base con una escena configurada en la que se encuentran los estantes en los que Amy podrá recoger cosas, las plataformas por las que deberá bajar Amy con las puertas que le dan acceso y el teletransportador. Tambien esta configurado el movimiento del personaje usando el raton y el teclado, usando para ello el componente cinemachine de Unity.

## Estantes y espaneo de cosas (GameManager)

Se suministra un prefab Thing de las cosas que debe manejar Amy, aunque incompleto.
Al comenzar el juego se deben espanear 20 cosas(Thing) en los 5 estantes (Shelf) existentes en la escena. Cada Thing espaneada debe asignarse a un Shelf escogido al azar.
Cada Shelf, por su parte deben recibir cada Thing que le es asignada, colocandola en el lugar que le corresponde.La 1º se colocara a una altura de 0.35m respecto al pivote del Shelf y cada Thing sucesiva 0.085 m mas arriba. Horizontalmente se colocarán centradas, por lo tanto en las coordenada 0 para los ejes X y Z.
El shelf debe, ademas llevar registro de las Thing que posee en un momento dado para poder ejecutar la funcionalidad que se explica mas abajo.

## Comportamiento de las Thing (Thing)
Cada Thing al ser espaneada elegira aleatoriamente entre los materiales disponibles cual adjudicarse. Este comportamiento es la parte que falta por completar del prefab Thing.

## Captura de las Thing por parte de Amy (Player)
Cuando el usuario pulse el boton Interaction Amy chequeara si tiene delante un objeto de la clase Shelf. De ser asi le pedira una Thing llamando a un metodo especifico para realizar esta tarea, que se deberá incluir en el comportamiento de Shelf.
La deteccion de que tiene delante un objeto Shelf, la hará Amy lanzando un RayCast desde un punto situado  una altura de 0.4f m, 0.1f m por delante del eje vertical del personaje, en direccion forward y con un alcance de 0.5fm.

El Shelf, por su parte buscará un objeto Thing en su registro y se lo entregará a Amy como resultado de la peticion. El Shelf debera eliminar la thing que acaba de entregar del registro que tiene en su poder. El Shelf ira entregando las Thing en el orden inverso en el que fueron entregadas a el, de manera que siempre entregara la que esta mas arriba en la pila que se ve en pantalla.
Se debera tratar el caso de que el Shelf ya no disponga de ninguna Thing y por tanto devuelva null como resultado de la peticion. Tambien se debera evitar todo este proceso en caso de que Amy ya tenga agarrada alguna Thing.

Si el Shelf le suministra un objeto Thing a Amy, esta lo agarrara haciendo que se mueva con ella, permaneciendo en una posicion(0f, 1f, 0.3f) en el espacio local de Amy con la orientacion identidad en este mismo espacio.
Además el personaje mostrará un faro(LightHouse) encima de su cabeza. Este consistira en una pequeña esfera de radio 0.1m situada en las coordenadas(0, 1.65f, 0). El faro ademas tomara, el color de la Thing que Amy haya agarrado, usando para ello su mismo material. Cuando Amy no tenga ningun objeto el faro debera permanecer invisible. El boton Interaction es necesario definirlo en la configuracion del proyecto, asociandolo a la tecla "e".

## Puerta de entrada a las plataformas (DoorMovement)

La puerta situada a la derecha(mirando hacia el muro) es la puerta de entrada a las plataformas donde se encuentra el teletransportador, y a donde debe dirigirse Amy tras haber cogido una Thing.
Esta puerta es el objeto InputDoubleDoor. Consiste en 2 hojas que deben desplazarse lateralmente, entre 2 posiciones definidas por los Transform LeftDoorClosedPoint, LeftDoorOpenedPoint para la hoja izquierda y RightDoorClosedPoint, RightDoorOpenedPoint para la hoja derecha. El movimiento de cada hoja entre cada una de las 2 posiciones debe realizarse en 1.5 s y debe ejecutarse con una aceleracion y frenado suaves, para lo que se recomienda el uso de la funcion SmoothStep()

Esta puerta debe abrirse en el momento en que Amy agarre una Thing, y deberá permanecer abierta hasta que Amy la atraviese, momento en el que se cerrara. La deteccion de que Amy la ha atravesado se hace con el BoxCollider en modo trigger incluido en el objeeto DoorCloser, hijo de InputDoubleDoor.

## Maquina de teletransporte (TranslateFloorUp)

En el pasillo horizontal, tras la 1º curva de plataformas, se encuentra la maquina de teletransporte. Cuando el jugador pulsa el boton "interaction", si Amy tiene una Thing agarrada y detecta delante de ella la maquina de teletransporte, destruira el objeto Thing, representando de este modo que la maquina no ha hecho nada, en realidad.
Para detectar la maquina de teletransporte se usara un Raycast de las mismas caracteristicas geometricas del usado para detectar los Shelf.

## Teletransporte de Amy (Player)
Para que Amy, siempre bajando en su camino por las plataformas, llegue de nuevo al lugar del que habia partido necesitamos un truco. El truco es teletransportar a Amy a una copia de la plataforma situada mas arriba, con la intencion de que el jugador no lo perciba. El teletransporte de Amy se hará al llegar al descansillo siguiente al pasillo de la maquina de teletransporte. La deteccion de la llegada de Amy a ese punto se hara mediante el correspondiente BoxCollider en modo trigger, incluido en el objeto Ramps/Landing2/Detector. En ese momento se llamara al metodo Translate() del script TranslateFloorUp que Amy tiene incorporado en el proyecto base.

## Puerta de salida de las plataformas (DoorMovement)
La puerta situada a la izquierda(mirando hacia el muro) es la puerta de salida de las plataformas y que se encontrará Amy al volver de su paseo por ellas. Esta puerta es geometricamente identica a la entrada y se mueve igual para abrir y cerrar. Se debe abrir cuando se acerca Amy, detectando esto en el BoxCollider situado junto a ella en el lado interior de la puerta, dentro del objeto DoorOpener y se debe cerrar en cuanto Amy la atraviese y toque el BoxCollider situado en el exterior de la misma, en el objeto DoorCloser.
