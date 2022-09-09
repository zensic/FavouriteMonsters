import {
  select,
  csv,
  scaleLinear,
  max,
  scaleBand,
  axisLeft,
  axisBottom,
  format
} from 'd3';

let teamSelected = [];

// Credits to https://stackoverflow.com/questions/105034/how-do-i-create-a-guid-uuid
const uuidv4 = () => {
  return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  )
};

// Remove monster from team when clicked on
const handleRemove = (id) => {
  for (let i = 0; i < teamSelected.length; i++) {
    if (teamSelected[i].tempId == id) {

      teamSelected.splice(i, 1);
      $("#" + id).remove();

      return;
    }
  };
}

const handleDetails = (id) => {
  $.ajax({
    type: 'GET',
    url: '/Teams/GetMonster',
    contentType: 'application/json; charset=utf-8',
    data: {id: id},
    success: function (result) {
      console.log(result);
    },
    error: function () {
      console.log('Failed ');
    }
  })
}

const handleAdd = (id, url, name, element) => {
  // A team can't have more than 6 monsters
  if (teamSelected.length > 5) {
    return 0;
  }

  // Generate unique uuid for each monster selected
  var tempId = uuidv4();

  // Add monster to the list to be pushed to cloud
  teamSelected.push({ 'tempId': tempId, 'monsterId': id });

  // Create component
  let component = $('<button/>', { 'id': tempId, 'class': 'monster-selected' })
    .append(
      $('<img />', { 'src': url, 'class': "new-monster-img", 'alt': 'Monster Image' })
    )
    .append(
      $('<div />').text(name)
    );

  // Insert compoenent
  $(component).insertBefore('#plus-selection');
  $('#' + tempId).click(() => { handleRemove(tempId) });

  // Hide the add button when team is full
  if (teamSelected.length > 5) {
    $('#plus-selection').css("display", "none");
  }
}

const handleSubmit = () => {
  console.log("Handle submit called");

  let TeamNew = {}; // To be pushed to the cloud
  let MonsterIds = []; // An attribute of TeamNew

  // Grab all information from teamSelected, append it to monsterIds
  for (let i = 0; i < teamSelected.length; i++) {
    MonsterIds.push(teamSelected[i].monsterId);
  }

  TeamNew.MonsterIds = MonsterIds;

  $.ajax({
    type: 'POST',
    url: '/Teams/Create',
    contentType: 'application/json; charset=utf-8',
    data: JSON.stringify(TeamNew),
    success: function (result) {
      alert('Successfully received Data ');
      console.log(result);
    },
    error: function () {
      alert('Failed to receive the Data');
      console.log('Failed ');
    }
  })
}