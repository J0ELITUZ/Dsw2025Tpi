# Trabajo Práctico Integrador
## Desarrollo de Software
### Backend
### INTEGRANTES
58400 Angel Joel Toledo Git (angeljoeltoledo@gmail.com) / INSTITUCIONAL Angel.Toledo@alu.frt.utn.edu.ar

56110 Luis Simon Bravo Luis.Bravo@alu.frt.utn.edu.ar

57874-Tolaba Milton Damian Maciel- Milton.Tolaba@alu.frt.utn.edu.ar

### Instrucciones para uso local 
1. Al abrir la solución iniciar la consola de comandos de Nuget.
2. Ejecutar el comando Update-Database.
3. Ahora que la base de datos está creada el programa está listo para ser ejecutado.

### Descripción de los endpoints
1. Crear un producto POST, solicita datos de producto al cliente y los almacena en la base de datos. Devuelve el objeto creado y el codigo 201.
2. Obtener todos los productos GET, consulta la tabla de productos de la base de datos y devuelve todos los productos cuya propiedad IsActive sea true y el codigo 200.
3. Obtener un producto por Id GET, solicita el id de un producto al cliente y devuelve el objeto producto que coincida con el id y el codigo 200.
4. Actualiza un producto PUT, solicita el id de un producto al cliente y los datos de modificación para el producto pertinente. Devuelve el objeto actualizado y el codigo 200.
5. Inhabilitar un producto PATCH, solicita el id de un producto que se desea inhabilitar, para cambiar la propiedad IsActive de este a false. Devuelve el codigo 204.
6. Agregar una orden POST, solicita customerId, y datos pertinentes de la orden asi tambien como los datos necesarios de cada orderItem para registrar la orden en la base de datos. Devuelve el objeto de la orden creada y el codigo 201.
