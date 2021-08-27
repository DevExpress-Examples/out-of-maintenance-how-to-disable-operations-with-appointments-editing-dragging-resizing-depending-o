<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128546450/15.2.14%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T466506)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebApplication1/Default.aspx) (VB: [Default.aspx](./VB/WebApplication1/Default.aspx))
* [Default.aspx.cs](./CS/WebApplication1/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebApplication1/Default.aspx.vb))
<!-- default file list end -->
# How to disable operations with appointments (editing, dragging, resizing) depending on a current user (an appointment's owner)
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/t466506/)**
<!-- run online end -->


<p>The base idea of a demonstrated approach is to store an appointment's owner (user) as an additional appointment's custom field.<br>This field value is stored automatically with a newly created appointment in the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_AppointmentInsertingtopic">ASPxScheduler.AppointmentInserting</a> event handler.<br><br>After that, this field value is passed from a server to a client (using the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_InitClientAppointmenttopic">ASPxScheduler.InitClientAppointment</a> event handler) and all operations with appointments are disabled depending on this property value in the <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerASPxScheduler_PopupMenuShowingtopic">ASPxScheduler.PopupMenuShowing</a>, <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerScriptsASPxClientScheduler_AppointmentDroptopic">ASPxClientScheduler.AppointmentDrop</a>, <a href="https://documentation.devexpress.com/#AspNet/DevExpressWebASPxSchedulerScriptsASPxClientScheduler_AppointmentResizetopic">ASPxClientScheduler.AppointmentResize</a> event handlers.<br><br>In this example, a current user can be selected from a combobox located on the top of the ASPxScheduler.<br>Appointments that cannot be edited by a current user are rendered with a "gray" background.</p>

<br/>


