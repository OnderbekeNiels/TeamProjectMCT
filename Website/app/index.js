var userId, userName;

function onSignIn(googleUser) {
        var profile = googleUser.getBasicProfile();
        console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
        console.log('Name: ' + profile.getName());
        console.log('Image URL: ' + profile.getImageUrl());
        console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.
        GetUserId(profile);
  }

const GetUserId = async function(profile){
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruikers/login?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  let data  = {"name": `${profile.getName()}`, "email": `${profile.getEmail()}`};
  console.log(data);

  fetch(endpoint, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(data),
  })
  .then((response) => response.json())
  //Then with the data from the response in JSON...
  .then((data) => {
    console.log('Success:', data);
    userId = data["gebruikerId"];
    console.log(userId);
    localStorage.setItem("gebruikerId", userId);
    userName = data["name"];
    console.log(userName);
    localStorage.setItem("name", userName);
    window.location.pathname = '/roundsoverview.html'
  })
  //Then with the error genereted...
  .catch((error) => {
    console.error('Error:', error);
  });
}


function signOut() {
    var auth2 = gapi.auth2.getAuthInstance();
    auth2.signOut().then(function () {
      console.log('User signed out.');
      window.location.pathname = '/index.html'
    });
  }


document.addEventListener("DOMContentLoaded", function(){
  console.log("DOM loaded");
}
)