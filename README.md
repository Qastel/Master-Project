# Master-Project


##PUBLIC
The application has been published at: https://educationalplatform.azurewebsites.net/.

##GITHUB
The application versions has been kept in Github at: https://github.com/Qastel/Master-Project

##LOCALLY
The application has been developed using Visual Studio 2017 and run locally from the IDE.


HOW TO RUN THE APP LOCALLY?
1. Install Visual Studio Community - Choose "ASP.NET and web development" Workload
2. Visual Studio may ask you to download ".NET Framework, Version= 4.6.2" when you open the project - if you do not want to manually install it
you can Keep the ".NET Framework, Version= 4.6.1"
3. Press the "IISExpress" button to run the application
4. In case the error "Could not find a part of the 'path bin\roslyn\csc.exe'" arise follow the instructions from: 
https://medium.com/@stbn200/solved-could-not-find-a-part-of-the-path-bin-roslyn-csc-exe-3c77d40f99e8   
(you can find the Nuget package console from menu, under "Tools">"Nuget Package Manager">"Nuget Package Console")


HOW TO USE THE APP?


The app is divided into 2 profiles: learner and instructor.

From the learner's View:
	1. You can read the Homepage and About page 
	2. Access Register Page and chose "learner Profile Type"
	3. Check "My Profile" Page (you can see account details and learned codebases)
	4. Access the "Codebases" page where you can find all the uploaded codebases with a biref description
	5. You can use the dropdown or search bar to filter the codebases (if no codebase is shown it means no codebase satisfies the options chose)
	6. Access the "Alien Invasion" codebase (some of other uploaded Codebases are dummy)
	7. Read the Codebases Details, you can save the codebase to your profile
	8. Read the Learning Instructions form where you can access the learning materials and the codebase
	9. On the chosen codebase page you can check the Reviews or write new ones for that specific page
	10. On the chosen codebase page you can access the Forum with all the topics and reponses for that codebase 
	(you can write or delete your own forum topics or reponses)

From the instructor's View:
	1. You can read the Homepage and About page 
	2. Access Register Page and chose "instructor Profile Type" (add your Qualification: Professor/Developer etc.)
	3. Check "My Profile" Page (you can see account details, uploaded and learned codebase)
	4. Access the "Codebases" page where you can find all the uploaded codebases with a brief description
	5. Access the "Instructor Training" page, this will contain the information from Chapter 2 of the report - Instructional Design Theory & Practice
	(it is aiming to train instructors on how to create the tasks)
	6. You need to use Google Forms for creating some tasks for a codebase that you want to be learned by students 
	7. Access the "Add New Codebase" and add the codebase details with the link to the Google Form that you created
	8. You can delete the uploaded codebase from "My Profile"
