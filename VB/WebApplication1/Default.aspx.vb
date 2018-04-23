Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports DevExpress.XtraScheduler

Namespace WebApplication1
	Partial Public Class [Default]
		Inherits System.Web.UI.Page
		Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

		End Sub

		Protected Sub ObjectDataSourceResources_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
			If Session("CustomResourceDataSource") Is Nothing Then
				Session("CustomResourceDataSource") = New CustomResourceDataSource(GetCustomResources())
			End If
			e.ObjectInstance = Session("CustomResourceDataSource")
		End Sub

		Private Function GetCustomResources() As BindingList(Of CustomResource)
			Dim resources As New BindingList(Of CustomResource)()
			resources.Add(CreateCustomResource(1, "Max Fowler"))
			resources.Add(CreateCustomResource(2, "Nancy Drewmore"))
			resources.Add(CreateCustomResource(3, "Pak Jang"))
			Return resources
		End Function

		Private Function CreateCustomResource(ByVal res_id As Integer, ByVal caption As String) As CustomResource
			Dim cr As New CustomResource()
			cr.ResID = res_id
			cr.Name = caption
			Return cr
		End Function

		Public RandomInstance As New Random()
		Private Function CreateCustomAppointment(ByVal subject As String, ByVal resourceId As Object, ByVal status As Integer, ByVal label As Integer, ByVal userName As String) As CustomAppointment
			Dim apt As New CustomAppointment()
			apt.Subject = subject
			apt.OwnerId = resourceId
			apt.StartTime = DateTime.Today.AddHours(label)
			apt.EndTime = apt.StartTime.AddHours(2)
			apt.Status = status
			apt.Label = label

			apt.AppointmentUser = userName
			Return apt
		End Function

		Protected Sub ObjectDataSourceAppointment_ObjectCreated(ByVal sender As Object, ByVal e As ObjectDataSourceEventArgs)
			If Session("CustomAppointmentDataSource") Is Nothing Then
				Session("CustomAppointmentDataSource") = New CustomAppointmentDataSource(GetCustomAppointments())
			End If
			e.ObjectInstance = Session("CustomAppointmentDataSource")
		End Sub

		Private Function GetCustomAppointments() As BindingList(Of CustomAppointment)
			Dim appointments As New BindingList(Of CustomAppointment)()

			Dim resources As CustomResourceDataSource = TryCast(Session("CustomResourceDataSource"), CustomResourceDataSource)
			If resources IsNot Nothing Then
				For Each item As CustomResource In resources.Resources
					Dim subjPrefix As String = item.Name & "'s "
					appointments.Add(CreateCustomAppointment(subjPrefix & "meeting", item.ResID, 2, 5, "User 1"))
					appointments.Add(CreateCustomAppointment(subjPrefix & "travel", item.ResID, 3, 3, "User 2"))
					appointments.Add(CreateCustomAppointment(subjPrefix & "phone call", item.ResID, 0, 10, "User 1"))
				Next item
			End If
			Return appointments
		End Function

		Protected Sub ASPxScheduler1_AppointmentViewInfoCustomizing(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.AppointmentViewInfoCustomizingEventArgs)
			If e.ViewInfo.Appointment.CustomFields("AppointmentUser") Is Nothing OrElse (e.ViewInfo.Appointment.CustomFields("AppointmentUser") IsNot Nothing AndAlso e.ViewInfo.Appointment.CustomFields("AppointmentUser").ToString() <> ASPxComboBoxUsers.Value.ToString()) Then
				e.ViewInfo.AppointmentStyle.BackColor = System.Drawing.Color.LightGray
				e.ViewInfo.AppointmentStyle.ForeColor = System.Drawing.Color.Gray
			End If
		End Sub

		Protected Sub ASPxScheduler1_InitClientAppointment(ByVal sender As Object, ByVal args As DevExpress.Web.ASPxScheduler.InitClientAppointmentEventArgs)
			args.Properties.Add("cpApptUser", args.Appointment.CustomFields("AppointmentUser"))
		End Sub

		Protected Sub ASPxScheduler1_PopupMenuShowing(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxScheduler.PopupMenuShowingEventArgs)
			If e.Menu.MenuId = SchedulerMenuItemId.AppointmentMenu Then
				e.Menu.ClientSideEvents.PopUp = "OnAppointmentMenuPopup"
			End If
		End Sub

		Protected Sub ASPxScheduler1_AppointmentInserting(ByVal sender As Object, ByVal e As PersistentObjectCancelEventArgs)
			TryCast(e.Object, Appointment).CustomFields("AppointmentUser") = ASPxComboBoxUsers.Value.ToString()
		End Sub
	End Class
End Namespace