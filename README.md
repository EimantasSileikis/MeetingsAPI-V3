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
