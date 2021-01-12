let userId;

userId = "547F309B-8596-4DBE-9439-333A7C9E79DE";

const secToTimeNotation = function (seconds) {
  let time = new Date(seconds * 1000).toISOString().substr(11, 8);
  return time;
};

const datetimeToDateNotation = function (date) {
  date = new Date(date);
  let day = date.getDate(),
    month = date.getMonth() + 1,
    year = date.getFullYear();
  if (day < 10) {
    day = `0${day}`;
  }
  if (month < 10) {
    month = `0${month}`;
  }
  return `${day}/${month}/${year}`;
};

const showTable = function (data) {
  console.table(data);
  data.sort((a, b) => (a.startDatum < b.startDatum) ? 1 : -1)
  const table = document.querySelector(".js-rounds-table");
  let htmlString = `<div class="c-ranking-table__header">
  <p class="c-ranking-table__header-item">
    Startdatum
  </p>
  <p class="c-ranking-table__header-item u-text-align--left">
    Ronde
  </p>
  <p class="c-ranking-table__header-item">
    Totale tijd
  </p>
  <p class="c-ranking-table__header-item">
    Positie
  </p>
</div>`;
  for (const item of data) {
    htmlString += `
        <div class="c-ranking-table__row js-rounds-table-row" data-roundid='${item.rondeId}'>
      <p class="c-ranking-table__row-item">
      ${datetimeToDateNotation(item.startDatum)}
      </p>
      <div class="c-ranking-table__row-item c-ranking-table__row-item--round-name u-text-align--left">
        <p class="c-ranking-table__row-item--top">${item.rondeNaam}</p>
        <p class="c-ranking-table__row-item--sub">${
          item.aantalEtappes
        } etappes</p>
      </div>
      <p class="c-ranking-table__row-item">
      ${secToTimeNotation(item.totaalTijd)}
      </p>
      <p class="c-ranking-table__row-item c-ranking-table__row-item--position u-color-alpha ">
      #${item.plaats}
      </p>
    </div>`;
  }
  table.innerHTML = htmlString;
};

const getRounds = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruiker/rondes/${userId}?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
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
  console.log("ID: " + profile.getId()); // Do not send to your backend! Use an ID token instead.
  console.log("Name: " + profile.getName());
  console.log("Image URL: " + profile.getImageUrl());
  console.log("Email: " + profile.getEmail()); // This is null if the 'email' scope is not present.
}

function signOut() {
  var auth2 = gapi.auth2.getAuthInstance();
  auth2.signOut().then(function () {
    console.log("User signed out.");
    window.location.pathname = "/index.html";
  });
}

document.addEventListener("DOMContentLoaded", function () {
  console.log("DOM loaded :)");
  // get userid from user:
  // userId = sessionStorage.getItem("gebruikerId");
  if(document.querySelector('.roundsoverview')){
    //getRounds();
  }
  if(document.querySelector('.etappesoverview')){
    console.log('Getting data etappa page');
  }
  
});
