Simple 911 - CAD / MDT Web Application

Built with 5 types of users in mind:
- Call Takers
- Dispatchers
- Ground Units
- Managers
- Administrators

Each part of the 911 process is split among the team to allow for the fastest dispatch times.

Ground Units view only the relevant information about an Incident.

Can be configured for Fire, EMS, Police, or all three together.

Current Features:
- Create Users and Assign Roles
- Call Takers Created Incident (Provide Address, Call Type, Priority)
- Dispatchers Assign a Ground Unit to Incident
- Call Taker may add Patient Info (Age, Sex, Breathing Status, Consciousness, Medical History)
- Both Call Takers & Dispatchers can add notes to Incident
- Incident Archive + Restore by Admin or Manager
- Status Buttons available for use by ONLY Assigned Ground Unit and Dispatcher
- Ground Units are prevented from adding notes and seeing the note-form-ui (Reduces Clutter, Maintains Easy-To-Read Page)
- Geo-Locate button. Locates Incident using Google Maps
- More small feautres

Road Map:
- Time Logger (Log Time for dispatch, enroute, onscene, etc..) (Has Time Created)
- Add multiple Units to a single Incident
- Unit Status instead of Incident Status (Or Both!)
- On-Screen Notifications for Ground Units when assigned a run
- Emergency / Mayday button

- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

Built on ASP.NET Core MVC. Using Npgsql to connect PostgreSQL to save and manage data. 
M-Peach is the sole-contributor of this project.
