using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.XtraScheduler;

namespace WebApplication1 {
    public partial class Default : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {

        }

        protected void ObjectDataSourceResources_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
            if(Session["CustomResourceDataSource"] == null) {
                Session["CustomResourceDataSource"] = new CustomResourceDataSource(GetCustomResources());
            }
            e.ObjectInstance = Session["CustomResourceDataSource"];
        }

        BindingList<CustomResource> GetCustomResources() {
            BindingList<CustomResource> resources = new BindingList<CustomResource>();
            resources.Add(CreateCustomResource(1, "Max Fowler"));
            resources.Add(CreateCustomResource(2, "Nancy Drewmore"));
            resources.Add(CreateCustomResource(3, "Pak Jang"));
            return resources;
        }

        private CustomResource CreateCustomResource(int res_id, string caption) {
            CustomResource cr = new CustomResource();
            cr.ResID = res_id;
            cr.Name = caption;
            return cr;
        }

        public Random RandomInstance = new Random();
        private CustomAppointment CreateCustomAppointment(string subject, object resourceId, int status, int label, string userName) {
            CustomAppointment apt = new CustomAppointment();
            apt.Subject = subject;
            apt.OwnerId = resourceId;
            apt.StartTime = DateTime.Today.AddHours(label);
            apt.EndTime = apt.StartTime.AddHours(2);
            apt.Status = status;
            apt.Label = label;

            apt.AppointmentUser = userName;
            return apt;
        }

        protected void ObjectDataSourceAppointment_ObjectCreated(object sender, ObjectDataSourceEventArgs e) {
            if(Session["CustomAppointmentDataSource"] == null) {
                Session["CustomAppointmentDataSource"] = new CustomAppointmentDataSource(GetCustomAppointments());
            }
            e.ObjectInstance = Session["CustomAppointmentDataSource"];
        }

        BindingList<CustomAppointment> GetCustomAppointments() {
            BindingList<CustomAppointment> appointments = new BindingList<CustomAppointment>();;
            CustomResourceDataSource resources = Session["CustomResourceDataSource"] as CustomResourceDataSource;
            if(resources != null) {
                foreach(CustomResource item in resources.Resources) {
                    string subjPrefix = item.Name + "'s ";
                    appointments.Add(CreateCustomAppointment(subjPrefix + "meeting", item.ResID, 2, 5, "User 1"));
                    appointments.Add(CreateCustomAppointment(subjPrefix + "travel", item.ResID, 3, 3, "User 2"));
                    appointments.Add(CreateCustomAppointment(subjPrefix + "phone call", item.ResID, 0, 10, "User 1"));                       
                }                    
            }
            return appointments;
        }

        protected void ASPxScheduler1_AppointmentViewInfoCustomizing(object sender, DevExpress.Web.ASPxScheduler.AppointmentViewInfoCustomizingEventArgs e) {
            if(e.ViewInfo.Appointment.CustomFields["AppointmentUser"] == null || (e.ViewInfo.Appointment.CustomFields["AppointmentUser"] != null && e.ViewInfo.Appointment.CustomFields["AppointmentUser"].ToString() != ASPxComboBoxUsers.Value.ToString())) {
                e.ViewInfo.AppointmentStyle.BackColor = System.Drawing.Color.LightGray;
                e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void ASPxScheduler1_InitClientAppointment(object sender, DevExpress.Web.ASPxScheduler.InitClientAppointmentEventArgs args) {
            args.Properties.Add("cpApptUser", args.Appointment.CustomFields["AppointmentUser"]);
        }

        protected void ASPxScheduler1_PopupMenuShowing(object sender, DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs e) {
            if(e.Menu.MenuId == SchedulerMenuItemId.AppointmentMenu) {
                e.Menu.ClientSideEvents.PopUp = "OnAppointmentMenuPopup";
            }        
        }

        protected void ASPxScheduler1_AppointmentInserting(object sender, PersistentObjectCancelEventArgs e) {
            (e.Object as Appointment).CustomFields["AppointmentUser"] = ASPxComboBoxUsers.Value.ToString();
        }
    }
}