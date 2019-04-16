using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using ATManager.Models;

namespace ATManager.Controllers
{
    public class HomeController : Controller
    {

        private AUTOSDUEntities db = new AUTOSDUEntities();
        //public ActionResult Index(string usr, string Opt1, string CercaTarga, string SearchLocation)
        //{
        //    bool isAuth = false;

        //    if (usr != String.Empty)
        //    {
        //        string UserName = "";

        //        string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
        //        HttpCookie cookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value); //Decrypt it
        //        UserName = ticket.Name; //You have the UserName!


        //        if (usr == UserName)
        //        {
        //            ViewBag.Messaggio = "BENE il cookie corrisponde!";
        //            //ViewBag.Messaggio = personaggio;
        //            isAuth = true;
        //            return RedirectToAction("DoRefresh", "Home");
        //        }
        //        else
        //        {
        //            ViewBag.Messaggio = "il cookie contenente lo 'username' non corrisponde allo User della queryString!";
        //            isAuth = false;
        //            return View("IncorrectLogin");
        //        }

        //    }
        //    return View();

        //}

        public ActionResult Index(string Opt1, string CercaTarga, string SearchLocation)
        {

            using (AUTOSDUEntities val = new AUTOSDUEntities())
            {
                //Session["Scelta1"] = "";

                var fromDatabaseEF = new SelectList(val.Luoghi_vw.ToList(), "ID", "DescrITA");
                ViewData["Luoghi"] = fromDatabaseEF;


            }

            if (String.IsNullOrEmpty(CercaTarga))
            {
                return View();
            }
            else if (!String.IsNullOrEmpty(CercaTarga))
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.Targa.ToString() == CercaTarga
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
            else
            {
                return View();
            }

            return RedirectToAction("DoRefresh", "Home");
        }

        public ActionResult DoRefresh(string Opt1, string CercaTarga, string SearchLocation)
        {
            if (CercaTarga != null && CercaTarga != "")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.Targa.ToString() == CercaTarga
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
            else if (Opt1 == "TUTTE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
           
            else if (Opt1 == "DA FARE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_SchedaTecnica == null
                            where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
            else if (Opt1 == "FATTE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_SchedaTecnica != null
                            where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }

            else
            {
                return View("Index");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contatti.";

            return View();
        }

        public ActionResult RitornaPannelloUtente()
        {
            string myParams = "Username=" + User.Identity.Name + "&param=0&from=1";

            return Redirect("http://192.168.20.1/Utente/UtentePanel?" + myParams);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("http://192.168.20.1/Utente/Login");

        }

        public ActionResult Edit(int ID)
        {
            AT_SchedaTecnica telai = db.AT_SchedaTecnica.Find(ID);

            return View(telai);
        }

        public ActionResult Create(string ID)
        {
            ViewBag.IDPerizia = ID;
            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr");
            ViewBag.IDStatoMezzo = new SelectList(db.AT_StatiMezzo, "ID", "Descr");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPerizia,IDTipoScheda,IDStatoMezzo,CE110,CE112,CE115,CE840,CE841,CE842,CE843,CE816," +
                                                   "CE265,CE135,CE160,CE145,CE150,CI820,CI825,CI835,CI837,CI1135")] AT_SchedaTecnica aT_SchedaTecnica)
        {
            if (ModelState.IsValid)
            {
                db.AT_SchedaTecnica.Add(aT_SchedaTecnica);
                db.SaveChanges();
                return RedirectToAction("DoRefresh", "Home");
            }

            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr",aT_SchedaTecnica.AT_TipiScheda);
            ViewBag.IDTipoScheda = new SelectList(db.AT_StatiMezzo, "ID", "Descr", aT_SchedaTecnica.AT_StatiMezzo);
            return View(aT_SchedaTecnica);
        }
    }
}