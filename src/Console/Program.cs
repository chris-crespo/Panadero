using Mascotas;
using Mascotas.UI.Console;
//using Mascotas.Data;
//using Mascotas.Models;

var view = new View();
// var system = new Manager(memberRepo, petRepo, specieRepo);
// var mapper = new Mapper();
var controller = new Controller(view, system/*, mapper*/);

#if !check
controller.Run();
#endif
