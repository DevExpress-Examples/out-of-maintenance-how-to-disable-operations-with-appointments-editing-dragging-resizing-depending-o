<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="WebApplication1.Default" %>

<%@ Register Assembly="DevExpress.Web.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.2, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>
<%@ Register Assembly="DevExpress.XtraScheduler.v15.2.Core, Version=15.2.20.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraScheduler" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
</head>
<body>
	<script type="text/javascript">
	    function OnAppointmentMenuPopup(s, e) {
	        var selectedApptIDs = clientScheduler.GetSelectedAppointmentIds();
	        var isOperationsAvailable = true;
	        if (selectedApptIDs.length > 0) {
	            for (var i = 0; i < selectedApptIDs.length; i++) {
	                if (!IsEditingAllowed(selectedApptIDs[0])) {
	                    isOperationsAvailable = false;
	                    break;
	                }
	            }
	            for (menuItemId in e.item.items) {
	                e.item.items[menuItemId].SetEnabled(isOperationsAvailable);
	            }
	        }
	    }

	    function OnAppointmentDropResize(s, e) {
	        var selectedApptIDs = clientScheduler.GetSelectedAppointmentIds();
	        if (selectedApptIDs.length > 0) {
	            for (var i = 0; i < selectedApptIDs.length; i++) {
	                if (!IsEditingAllowed(selectedApptIDs[i])) {
	                    e.operation.Cancel();
	                    e.handled = true;
	                }
	            }
	        }
	    }

		function IsEditingAllowed(apptID) {
			var currentAppt = clientScheduler.GetAppointmentById(apptID);
			return currentAppt.cpApptUser == cbUsers.GetValue();
		}
	</script>
	<form id="form1" runat="server">
		<div>
			<dx:ASPxComboBox ID="ASPxComboBoxUsers" runat="server" ValueType="System.String" ClientInstanceName="cbUsers">
				<Items>
					<dx:ListEditItem Selected="True" Text="User 1" Value="User 1" />
					<dx:ListEditItem Text="User 2" Value="User 2" />
				</Items>
				<ClientSideEvents SelectedIndexChanged="function (s, e) { clientScheduler.Refresh(); }" />
			</dx:ASPxComboBox>
			<dxwschs:ASPxScheduler ID="ASPxScheduler1" runat="server" AppointmentDataSourceID="ObjectDataSourceAppointment"
				ClientIDMode="AutoID" Start='<%#DateTime.Now%>' GroupType="Date" ClientInstanceName="clientScheduler"
				ResourceDataSourceID="ObjectDataSourceResources" OnInitClientAppointment="ASPxScheduler1_InitClientAppointment"
				OnAppointmentViewInfoCustomizing="ASPxScheduler1_AppointmentViewInfoCustomizing"
				OnAppointmentInserting="ASPxScheduler1_AppointmentInserting"
				OnPopupMenuShowing="ASPxScheduler1_PopupMenuShowing">
				<ClientSideEvents AppointmentDrop="OnAppointmentDropResize" AppointmentResize="OnAppointmentDropResize" />
				<OptionsCustomization AllowAppointmentConflicts="Forbidden" />
				<Storage>
					<Appointments AutoRetrieveId="True">
						<Mappings
							AllDay="AllDay"
							AppointmentId="Id"
							Description="Description"
							End="EndTime"
							Label="Label"
							Location="Location"
							ReminderInfo="ReminderInfo"
							ResourceId="OwnerId"
							Start="StartTime"
							Status="Status"
							Subject="Subject"
							Type="EventType" />
						<CustomFieldMappings>
							<dxwschs:ASPxAppointmentCustomFieldMapping Member="AppointmentUser" Name="AppointmentUser" ValueType="String" />
						</CustomFieldMappings>
					</Appointments>
					<Resources>
						<Mappings
							Caption="Name"
							ResourceId="ResID" />
					</Resources>
				</Storage>

				<Views>
					<DayView>
						<TimeRulers>
							<cc1:TimeRuler AlwaysShowTimeDesignator="False" ShowCurrentTime="Never"></cc1:TimeRuler>
						</TimeRulers>
						<DayViewStyles ScrollAreaHeight="600px">
						</DayViewStyles>
					</DayView>

					<WorkWeekView>
						<TimeRulers>
							<cc1:TimeRuler></cc1:TimeRuler>
						</TimeRulers>
					</WorkWeekView>
					<TimelineView>
						<CellAutoHeightOptions Mode="FitToContent" />
					</TimelineView>

					<FullWeekView>
						<TimeRulers>
							<cc1:TimeRuler></cc1:TimeRuler>
						</TimeRulers>
					</FullWeekView>
				</Views>
			</dxwschs:ASPxScheduler>
			<br />
			<br />
			<asp:Button ID="ButtonPostBack" runat="server" Text="Post Back" />

			<asp:ObjectDataSource ID="ObjectDataSourceResources" runat="server" OnObjectCreated="ObjectDataSourceResources_ObjectCreated" SelectMethod="SelectMethodHandler" TypeName="WebApplication1.CustomResourceDataSource"></asp:ObjectDataSource>
			<asp:ObjectDataSource ID="ObjectDataSourceAppointment" runat="server" DataObjectTypeName="WebApplication1.CustomAppointment" DeleteMethod="DeleteMethodHandler" InsertMethod="InsertMethodHandler" SelectMethod="SelectMethodHandler" TypeName="WebApplication1.CustomAppointmentDataSource" UpdateMethod="UpdateMethodHandler" OnObjectCreated="ObjectDataSourceAppointment_ObjectCreated"></asp:ObjectDataSource>
		</div>
	</form>
</body>
</html>