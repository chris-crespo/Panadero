﻿using Panadero;
using Panadero.UI.Console;
using Panadero.Data;
using Panadero.Models;

var productRepo = new ProductRepo();
var orderRepo = new OrderRepo();
var saleRepo = new SaleRepo();

var view = new View();
var mapper = new Mapper();
var system = new Sys(productRepo, orderRepo, saleRepo);
var controller = new Controller(view, system, mapper);

#if !check
controller.Run();
#endif
