let userId;

userId = "547F309B-8596-4DBE-9439-333A7C9E79DE";

const showTable = function (data) {
  console.table(data);
  const table = document.querySelector(".js-rounds-table");
  let htmlString = `<tr>
  <th class="c-ranking-table__header">Datum</th>
  <th class="c-ranking-table__header">Ronde</th>
  <th class="c-ranking-table__header">Totale Tijd</th>
  <th class="c-ranking-table__header">Positie</th>
</tr>`;
  for (const item of data) {
    let startDatum = new Date(item.startDatum);
    htmlString += `<tr>
    <td class="c-ranking-table__item">${startDatum.getDate()}/${startDatum.getMonth()}/${startDatum.getFullYear()}</td>
    <td class="c-ranking-table__item">${item.rondeNaam}</td>
    <td class="c-ranking-table__item">n/a</td>
    <td class="c-ranking-table__item">n/a</td>
  </tr>`;
  }
  table.innerHTML = htmlString;
};

const getRounds = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruikers/ronde/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    console.log(data);
    showTable(data);
  } catch (error) {
    console.error("An error occured, we handled it.", error);
  }
};

function onSignIn(googleUser) {
  var profile = googleUser.getBasicProfile();
  console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
  console.log('Name: ' + profile.getName());
  console.log('Image URL: ' + profile.getImageUrl());
  console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

}

function signOut() {
  var auth2 = gapi.auth2.getAuthInstance();
  auth2.signOut().then(function () {
    console.log('User signed out.');
    window.location.pathname = '/index.html'
  });
}

document.addEventListener("DOMContentLoaded", function () {
  console.log("DOM loaded :)");
  // get userid from user:
  userId = sessionStorage.getItem("gebruikerId");
  getRounds();
});
