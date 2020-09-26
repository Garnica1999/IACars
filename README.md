# IACars
Autos virtuales conducidos mediante algoritmos genéticos, redes neuronales y otra implementación de Q-learning construido en el motor grafico Unity2D.

## Implementacion

El proposito de este software es realizar una comparacion entre los algoritmos Q-learning, algoritmos geneticos y redes neuronales aplicado a la conduccion autonoma de vehiculos virtuales en 2D. 

* **Instancia:** Se instancian 30 vehiculos en la pista seleccioanda por el usuario. Este numero se puede cambiar facilmente desde el editor de Unity.

## Metodologia

**Selección de los algoritmos:** En esta fase se escogieron los algoritmos a utilizar en este trabajo, a partir de los siguientes criterios: facilidad de diseño e implementación, buen manejo de recursos computacionales y aprendizaje temprano de uno o mas agentes.
**Diseño y desarrollo de pistas y agentes:** En esta fase se diseñaron e implementaron las pistas y los agentes en Unity2D. Las pistas fueron inspiradas en entornos de conduccion competitiva en la vida real. La primera pista, *Ovalo*, es un entorno donde esta presente en mayor parte las rectas y curvas poco pronunciadas. La segunda pista, *Pista 1*, es un entorno donde esta presente multiples obstaculos, curvas mas pronunciadas y pocas rectas, entorno virutal el cual las tecnicas se les dificulta mas aprender y conducir un auto correctamente.
**Registro de medidas del experimento:** En esta etapa se diseño el experimento y los registros que se midieron para comparar los resultados.
**Análisis de los resultados:** Se realizó el análisis de los registros realizados y la comparación entre los algoritmos.

## Algoritmos utilizados

* **FNN-GA:** Se hizo un algoritmo hibrido entre redes neuronales y algoritmos geneticos. El algoritmo genetico tiene la funcion de optimizar los pesos de la red neuronal aplicada a cada vehiculo. Con esto no es necesario de implmenetar funciones de optimizacion como **SGD** o **adam**. Con esto permite una ejecucion mas rapida, permitiendo entrenar los autos mas facilmente en tiempo real.
* **Q-learning:** Es un algoritmo de aprendizaje por refuerzo. Este algoritmo sirve como apoyo para ver las diferencias en ambas pistas con respecto a la tecnica hibrida FNN-GA.

## Objetivo.

El objetivo es que un grupo de autos aprenda a conducir la pista sin chochar con los muros y obtaculos que estan presentes en ambas pistas.

## Tecnologias Utilizadas

Se escogio Unity por su facilidad para implementar fisicas y colisiones en un entorno virtual, lo que hace que la simulacion sea mas real. Aun asi, se sacrifica la falta de librerias dedicadas a machine learning, por lo que todos los modelos utilizados en este proyecto fueron construidas desde 0 o basandose en la literatura cientifica.

A continuacion, se describen todas las herramientas tecnologias que se utilizaron en este trabajo:

* Unity - Motor grafico

## Articulo cientifico

Proximamente se pondra el link del articulo cientifico publicado, haciendo referencia a este trabajo.

