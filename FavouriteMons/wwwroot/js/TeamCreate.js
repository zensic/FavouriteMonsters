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

// Adds a new component to the user interface
const handleAdd = (id, url, name) => {

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

// Renders details of current selected monster
const handleDetails = (id) => {
  $.ajax({
    type: 'GET',
    url: '/Teams/GetMonster',
    contentType: 'application/json; charset=utf-8',
    data: { id: id },
    success: function (result) {
      let resultJson = JSON.parse(result);

      // Store variables properly
      let name = resultJson['name'];
      let color = resultJson['color'];
      let url = resultJson['url'];
      let hp = resultJson['hp'];
      let attack = resultJson['attack'];
      let defence = resultJson['defence'];
      let speed = resultJson['speed'];

      // Render name and type
      $('.mons-create-name').text(resultJson['name']);
      $('.mons-create-dropdown')
        .text(resultJson['element'])
        .css("background-color", color);

      // Render image
      $('#new-monster-image').attr("src", url);

      // Clear previous click, add to team
      $('.mons-info-img-con').off('click');
      $('.mons-info-img-con').click(() => { handleAdd(id, url, name) });

      // Place monster stats into a list
      let statsList = [
        { stat: "HP", value: hp },
        { stat: "Attack", value: attack, },
        { stat: "Defence", value: defence, },
        { stat: "Speed", value: speed, }
      ];

      // Render monster stats from list
      const svg = d3.select('#mons-info-chart');

      // Clear previous chart
      svg.selectAll("*").remove();

      // + is equivalent to intParse
      const width = +svg.attr('width');
      const height = +svg.attr('height');

      const render = data => {

        // value accesors
        const xValue = d => d.value;
        const yValue = d => d.stat;

        // the margin convention
        const margin = { top: 20, right: 20, bottom: 20, left: 50 };
        const innerWidth = width - margin.left - margin.right;
        const innerHeight = height - margin.top - margin.bottom;

        // domain accepts min and d3.max values of dataset
        // range accepts min and d3.max values of screen size
        const xScale = d3.scaleLinear()
          .domain([0, 255])
          .range([0, innerWidth]);

        const yScale = d3.scaleBand()
          .domain(data.map(yValue))
          .range([0, innerHeight])
          .padding(0.2);

        // groups everything 
        const g = svg.append('g')
          .attr('transform', `translate(${margin.left}, ${margin.top})`)

        g.append('g').call(d3.axisLeft(yScale));
        g.append('g').call(d3.axisBottom(xScale))
          .attr('transform', `translate(0, ${innerHeight})`);

        g.selectAll('rect').data(data)
          .enter().append('rect')
          .attr('y', d => yScale(yValue(d)))
          .attr('width', d => xScale(xValue(d)))
          .attr('height', yScale.bandwidth())
      };

      // Render the chart
      render(statsList);
    },
    error: function () {
      console.log('Failed ');
    }
  })
}

// Posts data to the controller
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