# MeetingsAPI-V3

## Launching Web Service:

Clone git repository:
```
git clone --recurse https://github.com/Shilis/MeetingsAPI-V3.git
```
Navigate to the directory on command line and run command:
```
docker-compose up --build
```

Test api with swagger in browser
```
localhost:5000/swagger
```

## Soap

WSDL file
```
http://localhost:5000/meetings.asmx
```

### Get all meetings
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:GetMeetings/>
   </soapenv:Body>
</soapenv:Envelope>
```

### Get meeting with particular id
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:GetMeeting>
         <meet:meetingId>?</meet:meetingId>
      </meet:GetMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

### Create meeting

```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:AddMeeting>
         <meet:meeting>
            <meet:Name>?</meet:Name>
            <meet:Users>
               <!--Zero or more repetitions:-->
               <meet:User>
                  <!--Optional:-->
                  <meet:Id>?</meet:Id>
                  <meet:Surname>?</meet:Surname>
                  <meet:Name>?</meet:Name>
                  <meet:Number>?</meet:Number>
                  <meet:Email>?</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:meeting>
      </meet:AddMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```
Example:
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:AddMeeting>
         <meet:meeting>
            <meet:Name>Test</meet:Name>
            <meet:Users>
               <meet:User>
                  <meet:Id>11234</meet:Id>
                  <meet:Surname>Mer</meet:Surname>
                  <meet:Name>Eva</meet:Name>
                  <meet:Number>+37064787737</meet:Number>
                  <meet:Email>EvaMer@mail.com</meet:Email>
               </meet:User>
               <meet:User>
                  <meet:Surname>TestSurname</meet:Surname>
                  <meet:Name>TestName</meet:Name>
                  <meet:Number>+37051451651</meet:Number>
                  <meet:Email>test@mail.com</meet:Email>
               </meet:User>
               <meet:User>
                  <meet:Id>1</meet:Id>
                  <meet:Surname>Test2Surname</meet:Surname>
                  <meet:Name>Test2Name</meet:Name>
                  <meet:Number>+345365436</meet:Number>
                  <meet:Email>test2@mail.com</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:meeting>
      </meet:AddMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

### Update particular meeting
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:UpdateMeeting>
         <meet:id>?</meet:id>
         <meet:meetingDto>
            <meet:Name>?</meet:Name>
            <meet:Users>
               <!--Zero or more repetitions:-->
               <meet:User>
                  <meet:Id>?</meet:Id>
                  <meet:Surname>?</meet:Surname>
                  <meet:Name>?</meet:Name>
                  <meet:Number>?</meet:Number>
                  <meet:Email>?</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:meetingDto>
      </meet:UpdateMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```
Example:
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:UpdateMeeting>
         <meet:id>1</meet:id>
         <meet:meetingDto>
            <meet:Name>First Meeting</meet:Name>
            <meet:Users>
               <meet:User>
                  <meet:Id>87014</meet:Id>
                  <meet:Surname>UpdatedSurname</meet:Surname>
                  <meet:Name>UpdatedName</meet:Name>
                  <meet:Number>UpdatedNumber</meet:Number>
                  <meet:Email>UpdatedEmail</meet:Email>
               </meet:User>
               <meet:User>
                  <meet:Id>11234</meet:Id>
                  <meet:Surname>Mer</meet:Surname>
                  <meet:Name>Eva</meet:Name>
                  <meet:Number>+37064787737</meet:Number>
                  <meet:Email>EvaMer@mail.com</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:meetingDto>
      </meet:UpdateMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

### Delete particular meeting
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:DeleteMeeting>
         <meet:meetingId>?</meet:meetingId>
      </meet:DeleteMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

Example:
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:DeleteMeeting>
         <meet:meetingId>2</meet:meetingId>
      </meet:DeleteMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

### Add user to particular meeting
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:AddUserToMeeting>
         <meet:id>?</meet:id>
         <meet:user>
            <meet:Users>
               <meet:User>
                  <!--Optional:-->
                  <meet:Id>?</meet:Id>
                  <meet:Surname>?</meet:Surname>
                  <meet:Name>?</meet:Name>
                  <meet:Number>?</meet:Number>
                  <meet:Email>?</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:user>
      </meet:AddUserToMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

Example:
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:AddUserToMeeting>
         <meet:id>3</meet:id>
         <meet:user>
            <meet:Users>
               <meet:User>
                  <meet:Id>74638</meet:Id>
                  <meet:Surname>Dirk</meet:Surname>
                  <meet:Name>Mike</meet:Name>
                  <meet:Number>+37064787734</meet:Number>
                  <meet:Email>mikedirk@mail.com</meet:Email>
               </meet:User>
            </meet:Users>
         </meet:user>
      </meet:AddUserToMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

### Remove user from particular meeting
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:RemoveUserFromMeeting>
         <meet:id>?</meet:id>
         <meet:userId>?</meet:userId>
      </meet:RemoveUserFromMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```

Example:
```
<soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/" xmlns:meet="http://www.example.com/meetings">
   <soapenv:Header/>
   <soapenv:Body>
      <meet:RemoveUserFromMeeting>
         <meet:id>1</meet:id>
         <meet:userId>11234</meet:userId>
      </meet:RemoveUserFromMeeting>
   </soapenv:Body>
</soapenv:Envelope>
```
