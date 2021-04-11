ONU Shop Database Project
Created and Developed By: Dominic Ferrante (2020-2021)
Advised by: Dr. David Mikesell

Developer Contact Information:
Dominic Ferrante
d-ferrante@onu.edu

Link to public github repository: https://github.com/VaVoose/ShopDatabaseProject

INSTALLATION INSTRUCTIONS:
1. Open /Dependancies/x64
	(only built for x86 and x64 architecutres)
2. Double Click and install all of the dependancies in this folder
	(You may need manually close 'Skype' within the task manager to install some)
3. Do the same thing with the dependancies in /Dependancies/x86
4. Return to the main directory of the installer
5. Right click on the ShopDB_1.0.#.0_x86_x64_Debug.cer and select 'Install Certificate'
6. Select 'Local Machine' and continue
7. Allow Administrative Permissions
8. Select 'Place all Certificates in the following store'
9. Select Browse
10. Select the 'Trusted People' folder and click 'OK'
11. Select 'Next'
12. Select 'Finish'
13. You should get a popup saying 'The import was successful.', click 'OK'
14. Within the Windows Settings of the computer go to Settings->Update&Security->For Developers
15. Select the option 'Sideload apps'
16. Click Yes on the popup
17. In the main directory, double click on 'ShopDB_1.0.#.0_x86_x64_Debug.appxbundle
18. Click 'Install'
19. You will now find the application within the start menu on Windows
	(Right clicking it from the start menu will allow the option to pin it to the desktop or task bar)

~Notes~
The certificate to install the application expires on 4/11/2021. I have no idea how to make a longer lasting
certificate and I dont know if it will be able to be installed if the certificate is expired. 
So if you find yourself in that situation, you can either clone the repository on github and open it in 
Visual Studio and re-publish the application with your own certificate or contact me (Dominic) 
to see if I can create a new certificate or something.

INITIAL SETUP:
When the program is first installed a default admin user account is created with id '0'
Swipe a user ID to move to the add user page (do this with an account that you would like to make admin)
Enter in the users information here and create the user.
Go back to the starting page and type in "0" and click the continue button to log into the default admin account
Go to the users page and make the newly created user admin.
Log out of the '0' admin account
Log in with the newly admined account
delete the default '0' id admin account

TO ADD MACHINES:
Log in with an account that has admin privaledges
Select the 'Machines' option
At the top middle type in the the name of the new machine
Click the 'Add Machine' button

TO ASSIGN CERTIFICATIONS TO STUDENTS:
Log in with an admin account
Select the 'Users' option
On the user that you want to add a certification to, click the 'Edit User Certs' Button
(~Note~ You can delete users by selecting the 'Delete; button next to 'Edit User Certs' button)
From the dropdown at the top middle select the machine that is being certified
Click the 'Add Selected Machine' button
~Notes~
From this page as well you can update the date certified by clicking the 'Re-Up Cert.' button
You can remove certifications from users by clicking the 'Delete Cert.' button
From this page you can change the users first and last name if they initially entered in incorrectly when setting up
their account. Just type in the first and last name box and it automatically saves the new name

TO BACKUP AND RESTORE A DATABASE STATE:
Login with an account that is admin
TO BACKUP Select the 'Backup DB' option
Select the location that you want to save the back up
TO RESTORE Select the 'Restore DB' option
Navigate to the location of where the backed up database files are and select one and open it



 


