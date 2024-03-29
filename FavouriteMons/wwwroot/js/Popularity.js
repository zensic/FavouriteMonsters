﻿const svg = d3.select('#popular-monsters');

// + is equivalent to intParse
const width = +svg.attr('width');
const height = +svg.attr('height');

const render = data => {
  // value accesors
  const xValue = d => +d.likes;
  const yValue = d => d.monster;

  const margin = { top: 20, right: 20, bottom: 40, left: 40 };
  const innerWidth = width - margin.left - margin.right;
  const innerHeight = height - margin.top - margin.bottom;

  // domain accepts min and d3.max values of dataset
  // range accepts min and d3.max values of screen size
  const xScale = d3.scaleLinear()
    .domain([0, d3.max(data, xValue)])
    .range([0, innerWidth]);

  const yScale = d3.scaleBand()
    .domain(data.map(yValue))
    .range([0, innerHeight])
    .padding(0.1);

  const g = svg.append('g')
    .attr('transform', `translate(${margin.left}, ${margin.top})`)

  //const yAxis = d3.axisLeft(yScale);
  //yAxis(g.append('g'));
  g.append('g').call(d3.axisLeft(yScale))
    .attr('font-size', '16px');
  g.append('g').call(d3.axisBottom(xScale))
    .attr('transform', `translate(0, ${innerHeight})`)
    .attr('font-size', '12px');;

  g.selectAll('rect').data(data)
    .enter().append('rect')
    .attr('y', d => yScale(yValue(d)))
    .attr('width', d => xScale(xValue(d)))
    .attr('height', yScale.bandwidth())
};

// d3.csv makes a http req for data.d3.csv
// which makes a http req loads that d3.csv string
// parses that d3.csv string into an object
//d3.csv('data.d3.csv').then(data => {
//    data.forEach(d => {
//        d.population = +d.population;
//    });
//    render(data);
//});

const data = [
  { monster: "pingu", likes: 50 },
  { monster: "cat", likes: 800 },
  { monster: "dino", likes: 400 },
  { monster: "dog", likes: 900 }
];
render(data);