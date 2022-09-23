import React from 'react';
import './schedular.css';
import { registerLicense } from '@syncfusion/ej2-base';
import { ScheduleComponent, Inject , Day, Week , WorkWeek,Month,EventSettingsModel} from '@syncfusion/ej2-react-schedule';
import { DataManager,WebApiAdaptor } from '@syncfusion/ej2-data';

registerLicense('ORg4AjUWIQA/Gnt2VVhiQlFaclxJXGNWf1ZpR2NbfU5xflBBal1WVAciSV9jS3xTf0RmWX1ec3ZdQWZYVg==')
class App extends React.Component {
private localData: EventSettingsModel = {
  dataSource: [{
    End: new Date(2022,9, 15, 6, 30),
    Start: new Date(2022,8, 15 , 4, 0),
    Subject : '',
    //IsAllDay : true,
    RecurrenceRule : 'FREQ=DAILY;INTERVAL=1;COUNT=5',
  }],
  fields : {
    subject : {name:'Subject' , default:'No title provided'},
    startTime : {name : 'Start'},
    endTime : {name : 'End'}
  }
};
private remoteData = new DataManager({
  url: 'https://js.syncfusion.com/demos/ejservices/api/Schedule/LoadData', 
  adaptor: new WebApiAdaptor(), 
  crossDomain: true 
});


render() {
  return (
    <ScheduleComponent currentView='Month'
    selectedDate={new Date(2022,8,15)}
    eventSettings={ this.localData} > 
      <Inject services={[Day, Week, WorkWeek, Month]} />
    </ScheduleComponent>  
    
  );
}
}
export default App;
