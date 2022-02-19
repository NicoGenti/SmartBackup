# /Scripts/Config/VariabiliGlobali.psm1



**ATTENTION**
 When you set the Variables into VariabiliGlobali.psm1:

- **Don't delete " " ** In Section "# Variables set by User" 
  *Example:* 
``` powershell
$RFolderName = "NAME FOLDER R SCRIPT" #OLD
$RFolderName = "greece-prototype-r"	#NEW
```


- **Don't delete $ ** In Section "System Variables" 
  *Example:* 

``` powershell
$Global:useEmailSmartpeg = $true
```




  - 1 If you download the Zip, extract the Zip archive anywhere you want on your PC;

  - 2 navigate into subfolder *Config* and open *VariabiliGlobali.psm1* with text editor  for settings;

  - 3 set *$Global:rootFolder* with the **FULL PATH** of *DGRegioAutomation* Folder;
    
    ```powershell
    $Global:rootFolder = "YOUR FULL PATH OF AUTOMATION SCRIPT"
    ```
    
  - 4 set $RFolderName with the name of RScript folder (**NOT FULL PATH, ONLY NAME**);
    
    ```powershell
    $Global:RFolderName = "NAME FOLDER R SCRIPT"
    ```

  - 5 set FTP variables;
    
    ```powershell
    $Global:serverFTP = "YOUR SERVER FTP"
    $Global:userFTP = "YOUR USER FTP"
    $Global:passFTP = "YOUR PASSWORD FTP"
    ```
    
  - 6 set GIT variables (*ID Project and ID Repository can be retrieve trough API Devops*):
    
      ```powershell
    $Global:userGit = "DEVOPS USERNAME"
    $Global:passGIT = "DEVOPS TOKEN"
    $Global:IdProject = "ID PROJECT" 
    $Global:IdRepo = "ID REPOSITORY"
    $Global:organization = "ORGANIZATION NAME"
    $Global:gitBranch = "BRANCH GIT NAME"
    ```
    
  - 7 set parameter for Smartpeg Mail and Europe Mail *(About SendGrid Token view [SendGrid Guide](./SendGrid.md))*

    ```powershell
      - function Send-MailToSmartpeg() {
            Param (
                $Body
          )
    
      $Parameters = @{
          FromAddress = "EMAIL SMARTPEG SENDER ADDRESS"
          ToAddress   = "EMAIL ADMINISTRATOR RECEIVER ADDRESS"
          Subject     = "YOUR SUBJECT"
          Body        = $Body
          Token       = "YOUR SENDGRID TOKEN"
          FromName    = "NAME OF SENDER"
      }
      Send-PSSendGridMail @Parameters
      }
    
      function Send-MailToEurope() {
          Param (
              $Body
          )
    
      $Parameters = @{
          FromAddress = "EMAIL SMARTPEG SENDER ADDRESS"
          ToAddress   = "EMAIL EUROPE RECEIVER ADDRESS"
          Subject     = "YOUR SUBJECT"
          Body        = $Body
          Token       = "YOUR SENDGRID TOKEN"
          FromName    = "NAME OF SENDER"
      }
      Send-PSSendGridMail @Parameters
      }
    ```

**ONLY FOR EXPERT**

- It's possible enable or disable some program features:

  *Example:*

  ``` powershell
  $Global:useEmailSmartpeg = $true #set for send mail to Smartpeg
  $Global:useEmailSmartpeg = $false #set for don't send mail to Smartpeg
  ```

- When you move some system folder you need to write the new correct path in section "System Folders";
``` powershell
  $Global:outputFTPPath = '/output' #old
  $Global:outputFTPPath = '/files'  #new
```




# /Scripts/Config/qualityChecks2027.properties
- Into file qualityChecks2027.properties you need set the **FULL PATH** folder as in the example

```properties
# Cartelle utilizzate dall'applicazione per i processi di lettura e scrittura
folder.input-excel = /home/user/DGRegioAutomation/greece-prototype-r/xls-input
folder.export-csv = /home/user/DGRegioAutomation/greece-prototype-r/export-csv
folder.export-warnings =/ home/user/DGRegioAutomation/greece-prototype-r/export-warnings
```

