// selectNoteIndex: -1 for no select
// handlers.{onNoteMouseover,onNoteMousedown,onNoteMouseout,onNoteMouseup,onNoteDblclick}
function showRelationsSvg(id, nodes, links, width, height, allowModifyNote, allowModifyLink, selectNoteIndex, maxNoteId, handlers) {
    const colors = (d) => d.color;

    let lastNoteId = maxNoteId;

    var rawSvg = d3.select("#" + id)
        .on('contextmenu', () => { d3.event.preventDefault(); })
        .attr('width', width)
        .attr('height', height);

    var svg = rawSvg;

    // init D3 force layout
    const force = d3.forceSimulation()
        .force('link', d3.forceLink().id((d) => d.id).distance(150))
        .force('charge', d3.forceManyBody().strength(-500))
        .force('x', d3.forceX(width / 2))
        .force('y', d3.forceY(height / 2))
        .on('tick', tick);

    // init D3 drag support
    const drag = d3.drag()
        // Mac Firefox doesn't distinguish between left/right click when Ctrl is held... 
        .filter(() => d3.event.button === 0 || d3.event.button === 2)
        .on('start', (d) => {
            if (!d3.event.active) force.alphaTarget(0.3).restart();

            d.fx = d.x;
            d.fy = d.y;
        })
        .on('drag', (d) => {
            d.fx = d3.event.x;
            d.fy = d3.event.y;
        })
        .on('end', (d) => {
            if (!d3.event.active) force.alphaTarget(0);

            d.fx = null;
            d.fy = null;
        });

    // define arrow markers for graph links
    svg.append('svg:defs').append('svg:marker')
        .attr('id', 'end-arrow')
        .attr('viewBox', '0 -5 10 10')
        .attr('refX', 6)
        .attr('markerWidth', 3)
        .attr('markerHeight', 3)
        .attr('orient', 'auto')
        .append('svg:path')
        .attr('d', 'M0,-5L10,0L0,5')
        .attr('fill', '#000');

    svg.append('svg:defs').append('svg:marker')
        .attr('id', 'start-arrow')
        .attr('viewBox', '0 -5 10 10')
        .attr('refX', 4)
        .attr('markerWidth', 3)
        .attr('markerHeight', 3)
        .attr('orient', 'auto')
        .append('svg:path')
        .attr('d', 'M10,-5L0,0L10,5')
        .attr('fill', '#000');

    // line displayed when dragging new nodes
    const dragLine = svg.append('svg:path')
        .attr('class', 'link dragline hidden')
        .attr('d', 'M0,0L0,0');

    // handles to link and node element groups
    let path = svg.append('svg:g').selectAll('path');
    let circle = svg.append('svg:g').selectAll('g');

    // mouse event vars
    let selectedNote = null;
    let selectedLink = null;
    let mousedownLink = null;
    let mousedownNote = null;
    let mouseupNote = null;

    if (selectNoteIndex != -1) {
        selectedNote = nodes[selectNoteIndex];
    }

    function resetMouseVars() {
        mousedownNote = null;
        mouseupNote = null;
        mousedownLink = null;
    }

    // update force layout (called automatically each iteration)
    function tick() {
        // draw directed edges with proper padding from node centers
        path.attr('d', (d) => {
            const deltaX = d.target.x - d.source.x;
            const deltaY = d.target.y - d.source.y;
            const dist = Math.sqrt(deltaX * deltaX + deltaY * deltaY);
            const normX = deltaX / dist;
            const normY = deltaY / dist;
            const sourcePadding = d.left ? 17 : 12;
            const targetPadding = d.right ? 17 : 12;
            const sourceX = d.source.x + (sourcePadding * normX);
            const sourceY = d.source.y + (sourcePadding * normY);
            const targetX = d.target.x - (targetPadding * normX);
            const targetY = d.target.y - (targetPadding * normY);

            return `M${sourceX},${sourceY}L${targetX},${targetY}`;
        });

        circle.attr('transform', (d) => `translate(${d.x},${d.y})`);
    }

    // update graph (called when needed)
    function restart() {
        // path (link) group
        path = path.data(links);

        // update existing links
        path.classed('selected', (d) => d === selectedLink)
            .style('marker-start', (d) => d.left ? 'url(#start-arrow)' : '')
            .style('marker-end', (d) => d.right ? 'url(#end-arrow)' : '');

        // remove old links
        path.exit().remove();

        // add new links
        path = path.enter().append('svg:path')
            .attr('class', 'link')
            .classed('selected', (d) => d === selectedLink)
            .style('marker-start', (d) => d.left ? 'url(#start-arrow)' : '')
            .style('marker-end', (d) => d.right ? 'url(#end-arrow)' : '')
            .on('mousedown', (d) => {
                if (d3.event.ctrlKey) return;

                // select link
                mousedownLink = d;
                selectedLink = (mousedownLink === selectedLink) ? null : mousedownLink;
                selectedNote = null;
                restart();
            })
            .merge(path);

        // circle (node) group
        // NB: the function arg is crucial here! nodes are known by id, not by index!
        circle = circle.data(nodes, (d) => d.id);

        // update existing nodes (reflexive & selected visual states)
        circle.selectAll('circle')
            .style('fill', (d) => (d === selectedNote) ? d3.rgb(colors(d)).brighter().toString() : colors(d))
            .classed('reflexive', (d) => d.reflexive);

        // remove old nodes
        circle.exit().remove();

        // add new nodes
        const g = circle.enter().append('svg:g');

        g.append('svg:circle')
            .attr('class', 'node')
            .attr('r', 12)
            .style('fill', (d) => (d === selectedNote) ? d3.rgb(colors(d)).brighter().toString() : colors(d))
            .style('stroke', (d) => d3.rgb(colors(d)).darker().toString())
            .classed('reflexive', (d) => d.reflexive)
            .on('mouseover', function (d) {
                if (handlers !== undefined && handlers.onNoteMouseover !== undefined) {
                    handlers.onNoteMouseover(d);
                }

                if (!mousedownNote || d === mousedownNote) return;
                // enlarge target node
                d3.select(this).attr('transform', 'scale(1.1)');
            })
            .on('mouseout', function (d) {
                if (handlers !== undefined && handlers.onNoteMouseout !== undefined) {
                    handlers.onNoteMouseout(d);
                }

                if (!mousedownNote || d === mousedownNote) return;
                // unenlarge target node
                d3.select(this).attr('transform', '');
            })
            .on('mousedown', (d) => {
                if (handlers !== undefined && handlers.onNoteMousedown !== undefined) {
                    handlers.onNoteMousedown(d);
                }

                if (d3.event.ctrlKey) return;

                // select node
                mousedownNote = d;
                selectedNote = (mousedownNote === selectedNote) ? null : mousedownNote;
                selectedLink = null;

                if (allowModifyLink === true) {
                    // reposition drag line
                    dragLine
                        .style('marker-end', 'url(#end-arrow)')
                        .classed('hidden', false)
                        .attr('d', `M${mousedownNote.x},${mousedownNote.y}L${mousedownNote.x},${mousedownNote.y}`);
                }

                restart();
            })
            .on('mouseup', function (d) {
                if (handlers !== undefined && handlers.onNoteMouseup !== undefined) {
                    handlers.onNoteMouseup(d);
                }

                if (!mousedownNote || allowModifyLink === false) return;

                // needed by FF
                dragLine
                    .classed('hidden', true)
                    .style('marker-end', '');

                // check for drag-to-self
                mouseupNote = d;
                if (mouseupNote === mousedownNote) {
                    resetMouseVars();
                    return;
                }

                // unenlarge target node
                d3.select(this).attr('transform', '');

                // add link to graph (update if exists)
                // NB: links are strictly source < target; arrows separately specified by booleans
                const isRight = mousedownNote.id < mouseupNote.id;
                const source = isRight ? mousedownNote : mouseupNote;
                const target = isRight ? mouseupNote : mousedownNote;

                const link = links.filter((l) => l.source === source && l.target === target)[0];
                if (link) {
                    link[isRight ? 'right' : 'left'] = true;
                } else {
                    links.push({ source, target, left: !isRight, right: isRight });
                }

                // select new link
                selectedLink = link;
                selectedNote = null;
                restart();
            })
            .on('dblclick', function (d) {
                if (handlers !== undefined && handlers.onNoteDblclick !== undefined) {
                    handlers.onNoteDblclick(d);
                }
            });

        // show node IDs
        g.append('svg:text')
            .attr('x', 0)
            .attr('y', 4)
            .attr('class', 'id')
            .text((d) => d.id);

        circle = g.merge(circle);

        // set the graph in motion
        force
            .nodes(nodes)
            .force('link').links(links);

        force.alphaTarget(0.3).restart();
    }

    function mousedown() {
        // because :active only works in WebKit?

        svg.classed('active', true);

        if (d3.event.ctrlKey || mousedownNote || mousedownLink) return;

        if (allowModifyNote === false) return;

        // insert new node at point
        const point = d3.mouse(this);
        const node = { id: ++lastNoteId, reflexive: false, x: point[0], y: point[1] };
        nodes.push(node);

        restart();
    }

    function mousemove() {
        if (!mousedownNote) return;

        // update drag line
        dragLine.attr('d', `M${mousedownNote.x},${mousedownNote.y}L${d3.mouse(this)[0]},${d3.mouse(this)[1]}`);
    }

    function mouseup() {
        if (mousedownNote) {
            // hide drag line
            dragLine
                .classed('hidden', true)
                .style('marker-end', '');
        }

        // because :active only works in WebKit?
        svg.classed('active', false);

        // clear mouse event vars
        resetMouseVars();
    }

    function spliceLinksForNote(node) {
        const toSplice = links.filter((l) => l.source === node || l.target === node);
        for (const l of toSplice) {
            links.splice(links.indexOf(l), 1);
        }
    }

    // only respond once per keydown
    let lastKeyDown = -1;

    function keydown() {

        if (lastKeyDown !== -1) return;
        lastKeyDown = d3.event.keyCode;

        // ctrl
        if (d3.event.keyCode === 17) {
            circle.call(drag);
            svg.classed('ctrl', true);
            return;
        }

        if (!selectedNote && !selectedLink) return;

        switch (d3.event.keyCode) {
            case 8: // backspace
            case 46: // delete
                if (selectedNote && allowModifyNote === true) {
                    nodes.splice(nodes.indexOf(selectedNote), 1);
                    spliceLinksForNote(selectedNote);
                } else if (selectedLink && allowModifyLink === true) {
                    links.splice(links.indexOf(selectedLink), 1);
                }
                selectedLink = null;
                selectedNote = null;
                restart();
                break;
            case 66: // B
                if (selectedLink && allowModifyLink === true) {
                    // set link direction to both left and right
                    selectedLink.left = true;
                    selectedLink.right = true;
                }
                restart();
                break;
            case 76: // L
                if (selectedLink && allowModifyLink === true) {
                    // set link direction to left only
                    selectedLink.left = true;
                    selectedLink.right = false;
                }
                restart();
                break;
            case 82: // R
                if (selectedNote && allowModifyNote === true) {
                    // toggle node reflexivity
                    selectedNote.reflexive = !selectedNote.reflexive;
                } else if (selectedLink && allowModifyLink === true) {
                    // set link direction to right only
                    selectedLink.left = false;
                    selectedLink.right = true;
                }
                restart();
                break;
        }
    }

    function keyup() {
        lastKeyDown = -1;

        // ctrl
        if (d3.event.keyCode === 17) {
            circle.on('.drag', null);
            svg.classed('ctrl', false);
        }
    }

    // app starts here
    svg.on('mousedown', mousedown)
        .on('mousemove', mousemove)
        .on('mouseup', mouseup);
    d3.select(window)
        .on('keydown', keydown)
        .on('keyup', keyup);
    restart();
    return nodes;
}