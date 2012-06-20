﻿using System;
using System.Linq;
using System.Web.Mvc;
using DevBridge.Kern.Factories;

namespace DevBridge.Kern.Controllers
{
	public class AutocompleteController : Controller
	{
		public ActionResult Index(Type dataSourceType, string query)
		{
			var dataSource = AutocompleteDataSourceFactory.GetDataSource(dataSourceType);
			var items = dataSource.GetList(query).ToList();
			var retVal = new
			             	{
			             		query,
								suggestions = items.ToList().Select(c => c.Name).ToArray(),
								data = items.ToList().Select(c => c.Id).ToArray()
			             	};

			return Json(retVal);
		}
	}
}