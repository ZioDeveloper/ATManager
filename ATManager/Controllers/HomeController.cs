﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;
using ATManager.Models;
using System.Net;
using System.Data.Entity;
using System.IO;

namespace ATManager.Controllers
{
    public class HomeController : Controller
    {


        private AUTOSDUEntities db = new AUTOSDUEntities();
        //public ActionResult Index(string usr, string Opt1, string CercaTarga, string SearchLocation)
        //{

        //    if (usr != null)
        //        Session["User"] = usr;
        //    if (usr == null)
        //        usr = Session["User"].ToString();

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
        //            using (AUTOSDUEntities val = new AUTOSDUEntities())
        //            {
        //                Session["Status"] = "";

        //                var fromDatabaseEF = new SelectList(val.Luoghi_vw.ToList(), "ID", "DescrITA");
        //                ViewData["Luoghi"] = fromDatabaseEF;


        //            }

        //            if (String.IsNullOrEmpty(CercaTarga))
        //            {
        //                return View();
        //            }
        //            else if (!String.IsNullOrEmpty(CercaTarga))
        //            {
        //                var model = new Models.HomeModel();
        //                var telai = from s in db.AT_ListaPratiche_vw
        //                            where s.Targa.ToString() == CercaTarga
        //                            select s;
        //                model.AT_ListaPratiche_vw = telai.ToList();
        //                return View("ElencoTelai", model);
        //            }
        //            else
        //            {
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Messaggio = "il cookie contenente lo 'username' non corrisponde allo User della queryString!";
        //            isAuth = false;
        //            return View("IncorrectLogin");
        //        }

        //    }
        //    else
        //    {
        //        string UserName = "";

        //        string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
        //        HttpCookie cookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
        //        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value); //Decrypt it
        //        UserName = ticket.Name; //You have the UserName!
        //    }
        //    return View();


        //}

        public ActionResult Index(string Opt1, string CercaTarga, string SearchLocation)
        {

            using (AUTOSDUEntities val = new AUTOSDUEntities())
            {
                Session["Status"] = "";

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

            //return RedirectToAction("DoRefresh", "Home");
        }

        public ActionResult DoRefresh(string Opt1, string CercaTarga, string SearchLocation)
        {

            //string UserName = "";

            //string cookieName = FormsAuthentication.FormsCookieName; //Find cookie name
            //HttpCookie cookie = HttpContext.Request.Cookies[cookieName]; //Get the cookie by it's name
            //FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value); //Decrypt it
            //UserName = ticket.Name; //You have the UserName!

            if (Opt1 != null)
                Session["Status"] = Opt1;
            else
                Opt1 = Session["Status"].ToString();

            using (AUTOSDUEntities val = new AUTOSDUEntities())
            {
                //Session["Scelta1"] = "";

                var fromDatabaseEF = new SelectList(val.Luoghi_vw.ToList(), "ID", "DescrITA");
                ViewData["Luoghi"] = fromDatabaseEF;


            }

            if (CercaTarga != null && CercaTarga != "")
            {
                Session["TipoRicerca"] = "MONO";
                var IsInserted = (from s in db.AT_ListaPratiche_vw
                        where s.Targa.ToString() == CercaTarga
                        select s.Perizie_ID).Count();
                if (IsInserted > 0)
                {
                    try
                    {
                        var IsClosed = (from s in db.AT_ListaPratiche_vw
                                        where s.Targa.ToString() == CercaTarga
                                        select s.IsCompleted).FirstOrDefault();
                        if (IsClosed.Value == false)
                            Session["Status"] = "FATTE";
                        else
                            Session["Status"] = "FATTECHIUSE";
                    }
                    catch
                    {
                        Session["Status"] = "DA FARE";
                    }
                    
                   
                }
                else
                {
                    Session["Status"] = "DA FARE";
                }

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
                            //where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
           
            else if (Opt1 == "DA FARE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_SchedaTecnica == null
                            //where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
            else if (Opt1 == "FATTE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_SchedaTecnica != null
                            where s.IsCompleted == false
                            //where s.ID_LuogoIntervento == SearchLocation
                            select s;
                model.AT_ListaPratiche_vw = telai.ToList();
                return View("ElencoTelai", model);
            }
            else if (Opt1 == "FATTECHIUSE")
            {
                var model = new Models.HomeModel();
                var telai = from s in db.AT_ListaPratiche_vw
                            where s.ID_SchedaTecnica != null
                            where s.IsCompleted == true
                            //where s.ID_LuogoIntervento == SearchLocation
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

            return Redirect("https://webservices.interconsult.it/applogin/Utente/UtentePanel?" + myParams);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return Redirect("https://webservices.interconsult.it/applogin/");

        }

        //public ActionResult Edit(int ID)
        //{
        //    AT_SchedaTecnica telai = db.AT_SchedaTecnica.Find(ID);

        //    return View(telai);
        //}

        public ActionResult Create(string ID, string marca, string dataperizia, 
                                    string targa,string dataimmatricolazione,string km,
                                    string luogoperizia,string modello)
        {
            

            ViewBag.IDPerizia = ID;
            ViewBag.dataperizia = dataperizia;
            ViewBag.marca = marca;
            ViewBag.targa = targa;
            ViewBag.dataimmatricolazione = dataimmatricolazione;
            ViewBag.km = km;
            ViewBag.luogoperizia = luogoperizia;
            ViewBag.modello = modello;

            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr");
            ViewBag.IDStatoMezzo = new SelectList(db.AT_StatiMezzo, "ID", "Descr");
            ViewBag.IDPreventivoDanno = new SelectList(db.AT_PreventiviDanno, "ID", "Descr");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPerizia,IDTipoScheda,IDStatoMezzo,IDPreventivoDanno,IsCompleted,CE110,CE112,CE115,CE840,CE841,CE842,CE843,CE816," +
                                                   "CE265,CE135,CE160,CE145,CE150,CI820,CI825,CI835,CI837,CI1135, " +
                                                   "NoteCE110,NoteCE112,NoteCE115,NoteCE840,NoteCE841,NoteCE842,NoteCE843,NoteCE816," +
                                                   "NoteCE265,NoteCE135,NoteCE160,NoteCE145,NoteCE150,NoteCI820,NoteCI825,NoteCI835,NoteCI837,NoteCI1135," +
                                                   "Note_danno, Note_generali")] AT_SchedaTecnica aT_SchedaTecnica,string txtTitle)
        {



            if (ModelState.IsValid)
            {
                var sql = @" UPDATE SDU_PRATICHE SET Targa = @targa WHERE ID = @ID_pratica AND 0=1 ";
                var myScheda = from s in db.AT_ListaPratiche_vw
                               where s.Perizie_ID == aT_SchedaTecnica.ID
                select s.PRAT_ID;
                int IDPratica = myScheda.FirstOrDefault();

                int noOfRowInserted = db.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@ID_pratica", IDPratica),
                    new SqlParameter("@targa", txtTitle));


                db.AT_SchedaTecnica.Add(aT_SchedaTecnica);
                db.SaveChanges();
                return RedirectToAction("DoRefresh", "Home");
            }

            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr",aT_SchedaTecnica.AT_TipiScheda);
            ViewBag.IDStatoMezzo = new SelectList(db.AT_StatiMezzo, "ID", "Descr", aT_SchedaTecnica.AT_StatiMezzo);
            ViewBag.IDPreventivoDanno = new SelectList(db.AT_PreventiviDanno, "ID", "Descr", aT_SchedaTecnica.AT_PreventiviDanno);

            return View(aT_SchedaTecnica);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new Models.AT_ListaPratiche_vw();
            var myScheda = from s in db.AT_ListaPratiche_vw
                        where s.Perizie_ID == id
                        select s.Targa;
            model.Targa = myScheda.ToList().First();
            return View(model);
        }

        // POST: AT_SchedaTecnica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var sql = @"DELETE FROM  AT_SchedaTecnica WHERE IDPerizia = @IDPErizia";
            int myDeleted = db.Database.ExecuteSqlCommand(sql,  new SqlParameter("@IDPErizia", id));

            
            //AT_SchedaTecnica aT_SchedaTecnica = db.AT_SchedaTecnica.Find(id);
            //db.AT_SchedaTecnica.Remove(aT_SchedaTecnica);
            //db.SaveChanges();
            return RedirectToAction("DoRefresh");
        }

        public ActionResult Edit(int? id, string marca, string dataperizia,
                                    string targa, string dataimmatricolazione, string km,
                                    string luogoperizia, string modello,string blocked)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var model = new Models.AT_SchedaTecnica();
            var myScheda = from s in db.AT_SchedaTecnica
                           where s.IDPerizia == id
                           select s;
            model = myScheda.ToList().First();

            ViewBag.IDPerizia = id;
            ViewBag.dataperizia = dataperizia;
            ViewBag.marca = marca;
            ViewBag.targa = targa;
            ViewBag.dataimmatricolazione = dataimmatricolazione;
            ViewBag.km = km;
            ViewBag.luogoperizia = luogoperizia;
            ViewBag.modello = modello;
            ViewBag.blocked = blocked;
            //return View(model);


            //AT_SchedaTecnica aT_SchedaTecnica = db.AT_SchedaTecnica.Find(id);
            //if (aT_SchedaTecnica == null)
            //{
            //    return HttpNotFound();
            //}
            ViewBag.myIDScheda = model.ID;
            TempData["myIDScheda"] = model.ID;
            ViewBag.IDStatoMezzo = new SelectList(db.AT_StatiMezzo, "ID", "Descr", model.IDStatoMezzo);
            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr", model.IDTipoScheda);
            ViewBag.CE110 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE110);
            ViewBag.CE112 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE112);
            ViewBag.CE115 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE115);
            //ViewBag.CE125 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE125);
            ViewBag.CE135 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE135);
            ViewBag.CE145 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE145);
            ViewBag.CE150 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE150);
            ViewBag.CE160 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE160);
            //ViewBag.CE170 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE170);
            ViewBag.CE265 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE265);
            //ViewBag.CE415 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE415);
            ViewBag.CE816 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE816);
            ViewBag.CE840 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE840);
            ViewBag.CE841 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE841);
            ViewBag.CE842 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE842);
            ViewBag.CE843 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE843);
            //ViewBag.CE844 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE844);
            //ViewBag.CE960 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE960);
            //ViewBag.CI820 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI820);
            ViewBag.CI825 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI825);
            ViewBag.CI835 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI835);
            ViewBag.CI837 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI837);
            //ViewBag.CI843 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI843);
            //ViewBag.CI910 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI910);
            //ViewBag.CI930 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI930);
            //ViewBag.CS240 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS240);
            //ViewBag.CS510 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS510);
            //ViewBag.CS511 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS511);
            //ViewBag.CS710 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS710);
            //ViewBag.CS715 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS715);
            //ViewBag.CS810 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS810);
            //ViewBag.CS815 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CS815);
            //ViewBag.PS165 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS165);
            //ViewBag.PS225 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS225);
            //ViewBag.PS230 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS230);
            //ViewBag.PS235 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS235);
            //ViewBag.PS250 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS250);
            //ViewBag.PS260 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS260);
            //ViewBag.PS512 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS512);
            //ViewBag.PS610 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS610);
            //ViewBag.CE420 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CE420);
            //ViewBag.PS410 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.PS410);
            ViewBag.CI1135 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", model.CI1135);
            ViewBag.IDPreventivoDanno = new SelectList(db.AT_PreventiviDanno, "ID", "Descr", model.IDPreventivoDanno);
            ViewBag.NoteCE110 = model.NoteCE110;
            return View(model);
        }

        // POST: AT_SchedaTecnica/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPerizia,IDTipoScheda,IDStatoMezzo,IDPreventivoDanno,IsCompleted,CE110,CE112,CE115,CE840,CE841,CE842,CE843,CE816," +
                                                   "CE265,CE135,CE160,CE145,CE150,CI820,CI825,CI835,CI837,CI1135, " +
                                                   "NoteCE110,NoteCE112,NoteCE115,NoteCE840,NoteCE841,NoteCE842,NoteCE843,NoteCE816," +
                                                   "NoteCE265,NoteCE135,NoteCE160,NoteCE145,NoteCE150,NoteCI820,NoteCI825,NoteCI835,NoteCI837,NoteCI1135")] AT_SchedaTecnica aT_SchedaTecnica)
        {

            //var model = new Models.AT_SchedaTecnica();
            //var myScheda = from s in db.AT_SchedaTecnica
            //               where s.IDPerizia == aT_SchedaTecnica.ID
            //               select s;
            //model = myScheda.ToList().First();
            int myID = (int)TempData["myIDScheda"];



            if (ModelState.IsValid)
            {
                aT_SchedaTecnica.ID = myID;// model.ID;
                db.Entry(aT_SchedaTecnica).State = EntityState.Modified;
                
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("DoRefresh", "Home");
            }
            ViewBag.IDStatoMezzo = new SelectList(db.AT_StatiMezzo, "ID", "Descr", aT_SchedaTecnica.IDStatoMezzo);
            ViewBag.IDTipoScheda = new SelectList(db.AT_TipiScheda, "ID", "Descr", aT_SchedaTecnica.IDTipoScheda);
            ViewBag.CE110 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE110);
            ViewBag.CE112 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE112);
            ViewBag.CE115 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE115);
            //ViewBag.CE125 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE125);
            ViewBag.CE135 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE135);
            ViewBag.CE145 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE145);
            ViewBag.CE150 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE150);
            ViewBag.CE160 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE160);
            //ViewBag.CE170 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE170);
            ViewBag.CE265 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE265);
            //ViewBag.CE415 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE415);
            ViewBag.CE816 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE816);
            ViewBag.CE840 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE840);
            ViewBag.CE841 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE841);
            ViewBag.CE842 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE842);
            ViewBag.CE843 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE843);
            //ViewBag.CE844 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE844);
            //ViewBag.CE960 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE960);
            ViewBag.CI820 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI820);
            ViewBag.CI825 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI825);
            ViewBag.CI835 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI835);
            ViewBag.CI837 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI837);
            //ViewBag.CI843 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI843);
            //ViewBag.CI910 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI910);
            //ViewBag.CI930 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI930);
            //ViewBag.CS240 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS240);
            //ViewBag.CS510 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS510);
            //ViewBag.CS511 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS511);
            //ViewBag.CS710 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS710);
            //ViewBag.CS715 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS715);
            //ViewBag.CS810 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS810);
            //ViewBag.CS815 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CS815);
            //ViewBag.PS165 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS165);
            //ViewBag.PS225 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS225);
            //ViewBag.PS230 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS230);
            //ViewBag.PS235 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS235);
            //ViewBag.PS250 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS250);
            //ViewBag.PS260 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS260);
            //ViewBag.PS512 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS512);
            //ViewBag.PS610 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS610);
            //ViewBag.CE420 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CE420);
            //ViewBag.PS410 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.PS410);
            ViewBag.CI1135 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.CI1135);
            ViewBag.NoteCE110 = new SelectList(db.AT_IndiciValutazione, "ID", "Descr", aT_SchedaTecnica.NoteCE110);
            ViewBag.IDPreventivoDanno = new SelectList(db.AT_PreventiviDanno, "ID", "Descr", aT_SchedaTecnica.IDPreventivoDanno);
            return View(aT_SchedaTecnica);
        }

        public ActionResult FotoPerizia(int? ID)
        {

            ViewBag.IDPerizia = ID;
            return View();
        }

        public ActionResult FotoPratica(int? ID)
        {

            ViewBag.SDU_DocTipi = new SelectList(db.SDU_documentiTipi, "ID", "descrizioneDocumento");
            ViewBag.IDPerizia = ID;
            return View();
        }



        public ActionResult UploadFotoPerizia(IEnumerable<HttpPostedFileBase> files, int? ID)
        {
            
            string path = "";
            //var model = new Models.AT_ListaPratiche_vw();
            var myScheda = from s in db.AT_ListaPratiche_vw
                           where s.Perizie_ID == ID
                           select s.Perizie_Barcode; 
            string myBarcode = myScheda.ToList().First();
            
            var myScheda2 = from s in db.AT_ListaPratiche_vw
                           where s.Perizie_ID == ID
                           select s.Targa;
            string myTarga = myScheda2.ToList().First();


            string myDate = DateTime.Now.Year.ToString("0000") + 
                            DateTime.Now.Month.ToString("00") + 
                            DateTime.Now.Day.ToString("00") + 
                            DateTime.Now.Hour.ToString("00") + 
                            DateTime.Now.Minute.ToString("00") + 
                            DateTime.Now.Second.ToString("00");


            foreach (var file in files)
            {
                int fileCount = (from filecnt in Directory.EnumerateFiles(@"\\gewisfsrv\SDU\0036", myBarcode + "*.*", SearchOption.AllDirectories)
                                 select file).Count();

                path = System.IO.Path.Combine(@"\\gewisfsrv\SDU\0036\", myBarcode + "_" +
                                                                        myTarga + "_" +
                                                                        fileCount.ToString("000") + "_" +
                                                                        System.IO.Path.GetExtension(file.FileName));
                if (file != null)
                {
                    file.SaveAs(path);
                }

                

            }


            return RedirectToAction("DoRefresh", "Home");
        }

        public ActionResult UploadFotoPratica(IEnumerable<HttpPostedFileBase> files, int? ID , FormCollection form)
        {

            string path = "";
            string myIDTipoDoc = form["SDU_DocTipi"].ToString();

            var model = new Models.AT_ListaPratiche_vw();
            var myScheda = from s in db.AT_ListaPratiche_vw
                           where s.Perizie_ID == ID
                           select s.PRAT_ID; 
            int IDPratica = myScheda.ToList().First();
            string myDate = DateTime.Now.Year.ToString("0000") + 
                            DateTime.Now.Month.ToString("00") + 
                            DateTime.Now.Day.ToString("00") + 
                            DateTime.Now.Hour.ToString("00") + 
                            DateTime.Now.Minute.ToString("00") + 
                            DateTime.Now.Second.ToString("00");

            foreach (var file in files)
            {

                int fileCount = (from filecnt in Directory.EnumerateFiles(@"\\gewisfsrv\SDU\0036", IDPratica + "*.*", SearchOption.AllDirectories)
                                 select file).Count();
                fileCount++;

                path = System.IO.Path.Combine(@"\\gewisfsrv\SDU\0036\", IDPratica + "_" + 
                                                                        myDate + "_" + 
                                                                        fileCount.ToString("000") + "_" + 
                                                                        myIDTipoDoc + 
                                                                        System.IO.Path.GetExtension(file.FileName));
                


                if (file != null)
                {
                    file.SaveAs(path);
                }

                var sql = @"INSERT INTO SDU_documentiPratica (ID_pratica, ID_tipoDocumento,percorsoFile) Values (@ID_pratica, 
                                                                                                                 @ID_tipoDocumento, 
                                                                                                                 @percorsoFile)";
                int noOfRowInserted = db.Database.ExecuteSqlCommand(sql,
                    new SqlParameter("@ID_pratica", IDPratica),
                    new SqlParameter("@ID_tipoDocumento", myIDTipoDoc),
                    new SqlParameter("@percorsoFile",path));

            }



            return RedirectToAction("DoRefresh", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}