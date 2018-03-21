	var chars = [" ", "#", "$", "@", "^", "&", "*", "(", ")", "?", "/", "<", ">", ":", ",", ";", "'", "‘", "’", "\\", "|", "{", "}", "[", "]", "_", "~", "=", "-", ".", "´", "`", "¨"];

		var content = $("#content").html();

		function htmlDecode(inp) {
			var replacements = {'&lt;': '<', '&gt;': '>', '&sol;': '/', '&quot;': '"', '&apos;': '\'', '&amp;': '&', '&laquo;': '«', '&raquo;': '»', '&nbsp;': ' ', '&copy;': '©', '&reg;': '®', '&deg;': '°' };
			for (var r in replacements) {
				inp = inp.replace(new RegExp(r, 'g'), replacements[r]);
			}
			return inp;
		}
		content = htmlDecode(content);

		content = content.replace(/'s/g, "");
		$("#content").html(content);
		

        // Create array of start end end for each occurence
        function createArray() {
            var occurences = [];
            var length = 0;
            for (var i = 1; i <= $(".entityAIDA").length; i++) {
				var commonName = $(".entityAIDA:nth-child(" + i + ") > .cn").val();
				var freebaseId = $(".entityAIDA:nth-child(" + i + ") > .cn").next().next().val();
                var cn = $(".entityAIDA:nth-child(" + i + ") > .cn").val();
                for (var char in chars) {
                    while (cn.indexOf(chars[char]) > -1) {
						cn = cn.replace(chars[char], "");
					}
                }
                cn = cn.toLowerCase();
                for (var j = 1; j <= $(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA").length; j++) {
                    var startI = $(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA:nth-child(" + j + ") > .start").val();
                    var endI = $(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA:nth-child(" + j + ") > .end").val();
                    endI = parseInt(endI) + 1;
                    startI = parseInt(startI);
                    if (startI < 0) {
                        if (endI > commonName.length) {
							startI = endI - commonName.length;
							$(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA:nth-child(" + j + ") > .start").val(startI);
                        }
                        else {
							startI = endI = -1;
							if ($(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA").children().length === 1) {
								$(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA").remove();
							}
							else {
								$(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA:nth-child(" + j + ")").remove();
							}
                        }
                    }
                    if (occurences.findIndex(x => x.start === startI) === -1) {
                        if (startI >= 0) {
                            var list = {
								commonName: cn,
                                start: startI,
								end: endI,
								freebaseId: freebaseId
                            };
							occurences.push(list);
                        }
                    }
                    else {
						$(".entityAIDA:nth-child(" + i + ") > .occurencesAIDA > .occurenceAIDA:nth-child(" + j + ")").remove();
					}
				}
			}
            function mycomparator(a, b) {
                return parseInt(b.start, 10) - parseInt(a.start, 10);
            }
            occurences = occurences.sort(mycomparator);
			for (var occ in occurences) { length++; }
			for (var t = 1; t < length; t++) {
				if (occurences[t].end > occurences[t - 1].start) {
					var id = occurences[t].freebaseId;
					occurences.splice(t, 1);
					length--;
					if ($(".entityAIDA").find($('input[value="' + id + '"]')).next().children().length === 1) {
						var nameTag = $(".entityAIDA").find($('input[value="' + id + '"]')).parent().find($(".cn")).val();
						for (var char in chars) {
							while (nameTag.indexOf(chars[char]) > -1) {
								nameTag = nameTag.replace(chars[char], "");
							}
						}
						nameTag = nameTag.toLowerCase();
						$(".entityAIDA").find($('input[value="' + id + '"]')).parent().remove();
						// Delete description tag
						$("#imageAndDescriptionTag > ." + nameTag + "").remove();
						// Delete button Tag
						$(".tagsButton." + nameTag + "").remove();
						// Update select for add value to an existing tag
						$(".mySelect option:contains(" + nameTag + ")").remove();
					}
					else {
						$(".entityAIDA").find($('input[value="' + id + '"]')).next().find($('input[value="' + occurences[t].end + '"]')).parent().remove();
					}
				}
			}
            return occurences;
        }

        // Add color to text and buttons
        function addColor() {
            var content = $("#content").text().trim();

            var occurences = createArray();

            // Create spans
            for (var i = 0; i < occurences.length; i++) {
                // Create spans
                var toInsert = "<span id='s" + occurences[i].start + "' class='" + occurences[i].commonName + "'>";
                var start = occurences[i].start;
                var end = occurences[i].end;
                var contentEnd = content;
                var contentStart = contentEnd.slice(0, end) + "</span>" + contentEnd.slice(end);
                content = contentStart.slice(0, start) + toInsert + contentStart.slice(start);
                $("#content").html(content);
                $("#content span").addClass("sp");
            }

            // Add colors
            for (var a = 0; a < occurences.length; a++) {
                // Random colors
                var randomColor = "rgb(" +( Math.floor(Math.random() * 256)) + "," + (Math.floor(Math.random() * 256)) + "," + (Math.floor(Math.random() * 256)) + ")";

                $(".tagsButton." + occurences[a].commonName + "").css("background-color", randomColor);
                $("span." + occurences[a].commonName + "").css("background-color", randomColor).css("color", "white");

            }
        }
        addColor();

        function oneColor() {
			$(".oneTag").click(function (e) {
				// Remove colors
				$(".tagsButton").css("background-color", "cornflowerblue");
				$("span").css("background-color", "").css("color", "black");

				// Hide oneTag button and show See all tags button
				$(this).css("display", "none");
				$(".seeAllTags").css("display", "block");
			});
			$(".seeAllTags").click(function (e) {
				// Hide allTag button and show See one tags button
				$(this).css("display", "none");
				$(".oneTag").css("display", "block");

				addColor();
			});
        }
        oneColor();

        function colorAgain() {
			$(".recolor").click(function (e) {
				addColor();
			});
		}
        colorAgain();


        // Add names to the inputs in formUpdateAida
        function addNamesForm() {
            for (var b = 0; b < $(".entityAIDA").length; b++) {
                var name = "entity[" + b + "]";
                var child = b + 1;
                $(".entityAIDA:nth-child(" + child + ") input:nth-child(" + 1 + ")").attr("name", name + "commonName");
                $(".entityAIDA:nth-child(" + child + ") input:nth-child(" + 2 + ")").attr("name", name + "description");
                $(".entityAIDA:nth-child(" + child + ") input:nth-child(" + 3 + ")").attr("name", name + "freebaseId");
                for (var c = 0; c < $(".entityAIDA:nth-child(" + child + ") .occurencesAIDA").children().length; c++) {
                    var nameOcc = name + "occurences[" + c + "]";
                    var child2 = c + 1;
                    $(".entityAIDA:nth-child(" + child + ") .occurenceAIDA:nth-child(" + child2 + ") input:nth-child(" + 1 + ")").attr("name", nameOcc + "start");
                    $(".entityAIDA:nth-child(" + child + ") .occurenceAIDA:nth-child(" + child2 + ") input:nth-child(" + 2 + ")").attr("name", nameOcc + "end");
                    $(".entityAIDA:nth-child(" + child + ") .occurenceAIDA:nth-child(" + child2 + ") input:nth-child(" + 3 + ")").attr("name", nameOcc + "value");
                }
                for (var d = 0; d < $(".entityAIDA:nth-child(" + child + ") .imagesEntityAIDA").children().length; d++) {
                    var nameImg = name + "images[" + d + "]";
                    var child3 = d + 1;
                    $(".entityAIDA:nth-child(" + child + ") .imageEntityAIDA:nth-child(" + child3 + ") input:nth-child(" + 1 + ")").attr("name", nameImg + "title");
                    $(".entityAIDA:nth-child(" + child + ") .imageEntityAIDA:nth-child(" + child3 + ") input:nth-child(" + 2 + ")").attr("name", nameImg + "url");
                }
            }
            for (var e = 0; e < $(".imagesAIDA").children().length; e++) {
				$(".image" + e + " input:nth-child(" + 1 + ")").attr("name", "images[" + e + "]url");
				$(".image" + e + " input:nth-child(" + 2 + ")").attr("name", "images[" + e + "]name");
                $(".image" + e + " input:nth-child(" + 3 + ")").attr("name", "images[" + e + "]thumbnail");
            }
        }
        addNamesForm();


        // Show details tag
        function showDetails() {
		$('.tagsButton').click(function (e) {
			$('.details').attr("hidden", "true");
			$("#resultTarget").attr("hidden", "true");
			var valTag = $(this).text();
			for (var i = 0; i < chars.length; i++) {
				while (valTag.indexOf(chars[i]) > -1) {
					valTag = valTag.replace(chars[i], "");
				}
			}
			valTag = valTag.toLowerCase();
			$('.hideWhenSuggestions').removeAttr("hidden");
			$('#formReAnalyse').attr("hidden", "true");
			$("p." + valTag + "").removeAttr("hidden");
			$(".control").removeAttr("hidden");
			$("h4").removeAttr("hidden");
			$("div." + valTag + "").removeAttr("hidden");
			$(".inputCommonName").val($(this).text());
			$(".tagForThisDescription").html($(this).text());

			// if onecolor is clicked, color only one tag at the time
			if ($(".oneTag").css("display") === "none") {
				var myclass = $(this).attr("class");
				myclass = myclass.slice(17);
				// Remove colors for tags and spans already checked
				$(".tagsButton").css("background-color", "cornflowerblue");
				$("span").css("background-color", "").css("color", "black");
				// Add class to color the tag (button)
				$(this).css("background-color", "chartreuse");
				// Add class to color the span
				$("span." + myclass + "").css("background-color", "chartreuse").css("color", "white");
			}
		});
	}
        showDetails();


        // Show bigger image
        $(".imageSmall").click(function (e) {
			$(this).attr("hidden", "true");
			$(this).parent().find(".imageBig").removeAttr("hidden");
        });

        $(".imageBig").click(function (e) {
			$(this).attr("hidden", "true");
			$(this).parent().find(".imageSmall").removeAttr("hidden");
        });


        // Delete image
        $(".deleteImage").click(function (e) {
            // Update model for AIDA - start
            var classImage = $(this).parent().attr("class");
            $("." + classImage).remove();
            // Update model for AIDA - end
        });


        // Complete input selected value for tag with what the user selected
        function addTagOrValue() {
			$(".addNewTag").click(function (e) {
				var text = window.getSelection().toString().trim();
				if (text !== "") {
					$(".valueNewTag").val(text);
				}
				else {
					alert("Please select a value for the new tag first!");
					return false;
				}
			});

			$(".addTag").click(function (e) {
				// Hide the add to existing tag button and show the formulaire
				$(".formTag").removeAttr("hidden");
				$(".addTag").removeClass(" btn btn-primary active").attr("hidden", "true");
            });
        }
        addTagOrValue();


        // Add a value to an existing tag
        function addToExistingTag() {
		$(".addToExistingTag").click(function (e) {
			var text = window.getSelection().toString().trim();
			// Check if value tag is not empty
			if (text === "") {
				alert("Please choose a new value from the text");
			}
			else {
				var newValue = text;
				var tag = $(".mySelect").find('option:selected').text();
				var dataTag = $("button:contains(" + tag + ")").data("tags");
				dataTag = dataTag + newValue + "/";
				$("button:contains(" + tag + ")").attr("data-tags", dataTag);
				$(".addTag").addClass(" btn btn-primary active").removeAttr("hidden");
				$(".formTag").attr("hidden", "true");

				// Update model for AIDA - start
				// Create span around the value
				var sel = window.getSelection();
				if (sel.rangeCount) {
					var span = document.createElement('span');
					span.className = "add";
					span.textContent = text;
					var range = sel.getRangeAt(0);
					range.deleteContents();
					range.insertNode(span);
				}
				// Create div containing the occurence
				var occurence = document.createElement("div");
				occurence.className = "occurenceAIDA";
				// Select the right tag to place the occurence into
				var entityByCommonName = $(".entityAIDA").find($('*[value="' + tag + '"]'));
				var entity = entityByCommonName.parent();
				var occurences = entity.find($(".occurencesAIDA"));
				// Create input for value
				var value = document.createElement("input");
				value.setAttribute("type", "hidden");
				value.setAttribute("value", newValue);
				// Calculate value for start and value for end
				for (var i = 0; i < $(".occurenceAIDA").length; i++) {
					var contSpan = $("#content > span.sp:nth-child(1)").html();
					$("#content > span.sp:nth-child(1)").replaceWith(contSpan);
				}
				var content = $("#content").html();
				var startIndex = content.indexOf('<span class="add">');
				var endIndex = startIndex + $("span.add").text().length;
				endIndex = endIndex - 1;
				var rep = $("span.add").text();
				$("span.add").replaceWith(rep);
				// Create input for start
				var start = document.createElement("input");
				start.className = "start";
				start.setAttribute("type", "hidden");
				start.setAttribute("value", startIndex);
				// Create input for end
				var end = document.createElement("input");
				end.className = "end";
				end.setAttribute("type", "hidden");
				end.setAttribute("value", endIndex);
				// Add the inputs to occurence and the occurence to occurences
				occurence.appendChild(start);
				occurence.appendChild(end);
				occurence.appendChild(value);
				occurences.append(occurence);
				// Update model for AIDA - end

				createArray();
				addColor();
				addNamesForm();
			}
		});
	}
        addToExistingTag();


        // Check if a tag is selected when asking for suggestions
        $(".searchSuggestions").click(function (e) {
            if ($(".inputCommonName").val() === "") {
				alert("Please select a tag to show suggestions! \r\nIf you want to add a new tag, please select a value and 'Add New Tag'!");
				return false;
            }
        });


        // Hide description when suggestions are shown
        function hideDescriptionWhenSuggestions(e) {
			$('.hideWhenSuggestions').attr("hidden", "true");
		}

        // Show suggestions when received, don't show old results
        function showResultsDirectly(e) {
			$("#resultTarget").removeAttr("hidden");
		}


        // Add a new tag according to a selected suggestion
        $(document).on("click", "#selectSuggestion", function (e) {
            // Add the button for the new tag
            var commonName = $(this).parent().parent().find("h4").text();
            var dataTagNew = $(this).parent().parent().find("input").val();
            var id = $(this).parent().parent().find("h1").text();
            var descriptionTag = $(this).parent().parent().find("p").text();
            var srcImage = $(this).parent().parent().find("img").attr("src");
            var altImage = $(this).parent().parent().find("img").attr("alt");

            var commonNameNoChars = commonName;
            for (var i = 0; i < chars.length; i++) {
                while (commonNameNoChars.indexOf(chars[i]) > -1) {
					commonNameNoChars = commonNameNoChars.replace(chars[i], "");
				}
            }
            commonNameNoChars = commonNameNoChars.toLowerCase();
            var oldCommonName = dataTagNew;
            for (var s = 0; s < chars.length; s++) {
                while (oldCommonName.indexOf(chars[s]) > -1) {
					oldCommonName = oldCommonName.replace(chars[s], "");
				}
            }
            oldCommonName = oldCommonName.toLowerCase();
            dataTagNew += "/";

            // If new tag
            if (!$(".tagsButton").hasClass("" + oldCommonName + "")) {
                // Add the tag button
                var newTag = $("<button class='tagsButton label " + commonNameNoChars + "' >" + commonName + "</button > ");
                newTag.attr('data-tags', dataTagNew);
                newTag.attr('type', 'button');
                $(".allTags").append(newTag);

                // Add the description and photo for the tag
                var titleTag = "<p hidden class='" + commonNameNoChars + " details'><strong>" + commonName + "</strong></p>";
                $("#imageAndDescriptionTag").append(titleTag);
                if (srcImage !== undefined) {
                    var image = "<p hidden class='details " + commonNameNoChars + "'><img style='max-width:100%; max-height:30vh' src='" + srcImage + "' alt='" + altImage + "' /></p>";
                    $("#imageAndDescriptionTag").append(image);
                }
                var description = "<p hidden class='" + commonNameNoChars + " details hideUpdate'>" + descriptionTag + "</p>";
                $("#imageAndDescriptionTag").append(description);

                // When done, show description instead of suggestions
                $(".suggestions").attr("hidden", "true");
                $('.details').attr("hidden", "true");
                $("#resultTarget").attr("hidden", "true");
                $('.hideWhenSuggestions').removeAttr("hidden");
                $("h4").removeAttr("hidden");
                $(".control").removeAttr("hidden");
                $("p." + commonNameNoChars + "").removeAttr("hidden");

                // Update select for add value to an existing tag
                var option = $("<option>" + commonName + "</option > ");
                option.attr("value", commonNameNoChars);
                $(".mySelect").append(option);

                // Update model for AIDA - start
                // Create the div for the entity
                var entityAIDA = document.createElement("div");
                entityAIDA.className = "entityAIDA";

                // Create the 3 inputs for the general info entity
                var inputCN = document.createElement("input");
                var inputD = document.createElement("input");
                var inputFI = document.createElement("input");
                // Add the attributes to the inputs
                inputCN.className = "cn";
                inputCN.setAttribute("type", "hidden");
                inputCN.setAttribute("value", commonName);
                inputD.setAttribute("type", "hidden");
                inputD.setAttribute("value", descriptionTag);
                inputFI.setAttribute("type", "hidden");
                inputFI.setAttribute("value", id);
                // Append the inputs to the div for entityAIDA
                entityAIDA.appendChild(inputCN);
                entityAIDA.appendChild(inputD);
                entityAIDA.appendChild(inputFI);

                // Function to return all the indexesOf a value can be found in the context
                function indexesOf(source, find) {
                    var result = [];
                    for (var i = 0; i < source.length; i++) {
                        var fin = i + find.length;
                        if (source.substring(i, fin) === find) {
							result.push(i);
						}
                    }
                    return result;
                }

                // Create the div for occurences
                var occurences = document.createElement("div");
                occurences.className = "occurencesAIDA";
                // Select the context (necessary to find where the value(s) is(are) in the text)
				var context = $(".contentUnmodified").val();
				context = htmlDecode(context);
				context = context.replace(/'s/g, "");
                var name = dataTagNew.slice(0, -1);
                var indexes = indexesOf(context, name);
                for (var p = 0; p < indexes.length; p++) {
					var startNT = indexes[p];
                    var endNT = startNT + name.length - 1;
                    var occurence = document.createElement("div");
                    occurence.className = "occurenceAIDA";
                    // Create input for value
                    var valueO = document.createElement("input");
                    valueO.setAttribute("type", "hidden");
                    valueO.setAttribute("value", name);
                    // Create input for start
                    var startO = document.createElement("input");
                    startO.className = "start";
                    startO.setAttribute("type", "hidden");
                    startO.setAttribute("value", startNT);
                    // Create input for end
                    var endO = document.createElement("input");
                    endO.className = "end",
                    endO.setAttribute("type", "hidden");
                    endO.setAttribute("value", endNT);
                    // Add the inputs to occurence and the occurence to occurences
                    occurence.appendChild(startO);
                    occurence.appendChild(endO);
                    occurence.appendChild(valueO);
                    occurences.append(occurence);
                }
                // Add the div to entity
                entityAIDA.appendChild(occurences);

                // Create the div for images
                var imagesEntity = document.createElement("div");
				imagesEntity.className = "imagesEntityAIDA";
				if (srcImage !== undefined) {
					// Create inner div for image
					var imageEntity = document.createElement("div");
					imageEntity.className = "imageEntityAIDA";
					// Create inputs for the imageEntity
					var inputT = document.createElement("input");
					var inputU = document.createElement("input");
					// Add attributes to inputs
					inputT.setAttribute("type", "hidden");
					inputT.setAttribute("value", altImage);
					inputU.setAttribute("type", "hidden");
					inputU.setAttribute("value", srcImage);
					// Append the inputs to the div for imageAIDA
					imageEntity.appendChild(inputT);
					imageEntity.appendChild(inputU);
					// Append the inner div to the div imagesEntity
					imagesEntity.appendChild(imageEntity);
				}
                // Add the div to entity
                entityAIDA.appendChild(imagesEntity);

                // Append the div entityAIDA to Entities
                $("#sementisEntitiesAIDA").append(entityAIDA);
                // Update model for AIDA - end

                // Add color to tag and value and show details
                createArray();
                addColor();
                colorAgain();
                oneColor();
                addNamesForm();
                showDetails();
            }

            // If updating description for tag
            else {
				$("#imageAndDescriptionTag").find($(".details." + oldCommonName + "")).remove();

				// Change the tag
				$(".allTags").find($("." + oldCommonName + "")).html(commonName);

                // Update select for add value to an existing tag
                $('.mySelect option[value="' + oldCommonName + '"]').attr("value", commonNameNoChars).html(commonName);

                // Update class button and class span
                $(".allTags").find($("." + oldCommonName + "")).removeClass(oldCommonName).addClass(commonNameNoChars);
                var content = $("#content").text();
                $("#content").find($("." + oldCommonName + "")).removeClass(oldCommonName).addClass(commonNameNoChars);

                // Add the description and photo for the tag
                var titleITag = "<p hidden class='" + commonNameNoChars + " details'><strong>" + commonName + "</strong></p>";
                $("#imageAndDescriptionTag").append(titleITag);
                if (srcImage !== undefined) {
                    var pImage = "<p hidden class='details " + commonNameNoChars + "'><img style='max-width:100%; max-height:30vh' src='" + srcImage + "' alt='" + altImage + "' /></p>";
                    $("#imageAndDescriptionTag").append(pImage);
                }
                var descriptionT = "<p hidden class='" + commonNameNoChars + " details hideUpdate'>" + descriptionTag + "</p>";
                $("#imageAndDescriptionTag").append(descriptionT);

                // When done, show description instead of suggestions
                $(".suggestions").attr("hidden", "true");
                $("#resultTarget").attr("hidden", "true");
                $('.hideWhenSuggestions').removeAttr("hidden");
                $("p." + commonNameNoChars + "").removeAttr("hidden");

                // Update model for AIDA - start
                // Modify the general info about the tag
                var nameInput = "entity[i]commonName";
                var ii = $(".allTags").find($("." + commonNameNoChars + "")).prevAll().length;
                nameInput = nameInput.replace("[i]", "[" + ii + "]");
                $('.entityAIDA').find($('input[name="' + nameInput + '"]')).val(commonName);
                $('.entityAIDA').find($('input[name="' + nameInput + '"]')).next().val(descriptionTag);
                $('.entityAIDA').find($('input[name="' + nameInput + '"]')).next().next().val(id);

                // Modify the image for the entity
                $('.entityAIDA').find($('input[name="' + nameInput + '"]')).parent().find(".imagesEntityAIDA").empty();
                if (srcImage !== undefined) {
                    // Create div image
                    var divI = document.createElement("div");
                    divI.className = "imageEntityAIDA";
                    // Create inputs image
                    var inputN = document.createElement("input");
                    var inputUE = document.createElement("input");
                    // Add the attributes to the inputs
                    inputN.setAttribute("type", "hidden");
                    inputN.setAttribute("value", altImage);
                    inputUE.setAttribute("type", "hidden");
                    inputUE.setAttribute("value", srcImage);
                    // Append the inputs to the div for image
                    divI.appendChild(inputN);
                    divI.appendChild(inputUE);
                    $('.entityAIDA').find($('input[name="' + nameInput + '"]')).parent().find(".imagesEntityAIDA").append(divI);
                }
                // Update model for AIDA - end

                addNamesForm();
            }
        });


        // Delete a value from text if wrongly tagged
        $(".deleteValue").click(function (e) {
            var text = window.getSelection().toString();
            var sel = window.getSelection();
            if (text === "") {
				alert("Please select a value to remove first!");
				return false;
            }
            else {
                // Create span around the value to delete
                if (sel.rangeCount) {
                    var span = document.createElement('span');
                    span.className = "delete";
                    span.textContent = text;
                    var range = sel.getRangeAt(0);
                    range.deleteContents();
                    range.insertNode(span);
                }

                // Delete the original span around the value
				var idSpan = $("span.delete").parent().attr("id");
				var classSpan = $("span.delete").parent().attr("class");
                var content = $("#content").text();
                var keep = $("span#" + idSpan).text();
                $("span#" + idSpan).replaceWith(keep);

				// Check if it's the last value. If yes, remove the entity
				var start = idSpan.slice(1);
				classSpan = classSpan.substring(0, classSpan.length - 3);
				if ($(".occurenceAIDA").find($('input[value="' + start + '"]')).parent().parent().children().length === 1) {
					// Delete description tag
					$("#imageAndDescriptionTag").find($(".details." + classSpan + "")).remove();

					// Delete button Tag
					var val = $(".tagsButton." + classSpan + "").text();
					$(".tagsButton." + classSpan + "").remove();

					// Update select for add value to an existing tag
					$(".mySelect option:contains(" + val + ")").remove();

					// Update model for AIDA - start
					$(".occurenceAIDA").find($('input[value="' + start + '"]')).parent().parent().parent().remove();
					// Update model for AIDA - end
				}
				else {
					// Update model for AIDA - start
					$(".occurenceAIDA").find($('input[value="' + start + '"]')).parent().remove();
					// Update model for AIDA - end
				}

                createArray();
                addColor();
            }
        });


        // Delete tag from menu delete
        $(".delete").click(function (e) {
            var alerted = localStorage.getItem('alerted') || '';
            if (alerted !== 'yes') {
				alert("This will delete your tag! \r\nIf you only want to change the description for your tag, please select 'Suggestions'!");
				localStorage.setItem('alerted', 'yes');
            }
            else {
                var tagToDelete = $(this).parent().parent().find($(".inputCommonName")).val();
                for (var i = 0; i < chars.length; i++) {
                    while (tagToDelete.indexOf(chars[i]) > -1) {
						tagToDelete = tagToDelete.replace(chars[i], "");
					}
                }
                tagToDelete = tagToDelete.toLowerCase();

                // Delete description tag
                $("#imageAndDescriptionTag").find($(".details." + tagToDelete + "")).remove();

                // Delete button Tag
                $(".tagsButton." + tagToDelete + "").remove();

                // Delete the span with the value from content
                var textSpan = $('#content').find($("span." + tagToDelete + "")).text();
                var spanToDelete = $('#content').find($("span." + tagToDelete + ""));
                var contentToModify = $("#content").text();
                contentToModify.replace(spanToDelete, textSpan);
                $("#content").html(contentToModify);

                // Hide the description
                $('.hideWhenSuggestions').attr("hidden", "true");

                // Update select for add value to an existing tag
                $(".mySelect option:contains(" + $(this).parent().parent().find($(".inputCommonName")).val() + ")").remove();

                // Update model for AIDA - start
                var tagDelete = $(this).parent().parent().find($(".inputCommonName")).val();
                $(".entityAIDA").find($('input[value="' + tagDelete + '"]')).parent().remove();
                // Update model for AIDA - end

                // Re-color
                createArray();
                addColor();
                addNamesForm();
            }
        });

        // Delete tag from delete tag under description
        $(".deleteTag").click(function (e) {
            var alerted = localStorage.getItem('alerted') || '';
            if (alerted !== 'yes') {
				alert("This will delete your tag! \r\nIf you only want to change the description for your tag, please select 'Suggestions'!");
				localStorage.setItem('alerted', 'yes');
            }
            else {
                // Delete description tag
                var tagToDelete = $(this).parent().parent().find($(".tagForThisDescription")).text();
                for (var i = 0; i < chars.length; i++) {
                    while (tagToDelete.indexOf(chars[i]) > -1) {
						tagToDelete = tagToDelete.replace(chars[i], "");
					}
                }
                tagToDelete = tagToDelete.toLowerCase();
                $("#imageAndDescriptionTag").find($(".details." + tagToDelete + "")).remove();

                // Delete button Tag
                $(".tagsButton." + tagToDelete + "").remove();

                // Delete the span with the value from content
                var textSpan = $('#content').find($("span." + tagToDelete + "")).text();
                var spanToDelete = $('#content').find($("span." + tagToDelete + ""));
                var contentToModify = $("#content").text();
                contentToModify.replace(spanToDelete, textSpan);
                $("#content").html(contentToModify);

                // Hide the deleted description
                $('.hideWhenSuggestions').attr("hidden", "true");

                // Update select for add value to an existing tag
                $(".mySelect option:contains(" + $(this).parent().parent().find($(".tagForThisDescription")).text() + ")").remove();

                // Update model for AIDA - start
                var tagDelete = $(this).parent().parent().find($(".tagForThisDescription")).text();
                $(".entityAIDA").find($('input[value="' + tagDelete + '"]')).parent().remove();
                // Update model for AIDA - end

                // Re-color
                createArray();
                addColor();
                addNamesForm();
            }
        });


        // Show form, hide other info and add the text to re-analyse
		$(".reAnalyse").click(function (e) {
			$("#formReAnalyse").removeAttr("hidden");
			$("#resultTarget").attr("hidden", "true");
			$(".hideWhenSuggestions").attr("hidden", "true");

			$(".textToAnalyse").val("");
			var text = window.getSelection().toString();
			var content = $("#content").text();
			if (text !== undefined) {
				$(".textToAnalyse").val(text);
			}
			else {
				$(".textToAnalyse").val(content);
			}
		});

        // Check if text exists before sumitting
        function checkTextToReAnalyse(e) {
            if ($(".textToAnalyse").val() === "") {
				alert("Please add the text to be re-analysed!");
				e.preventDefault();
            }

            // Show the text with please wait
            $(".pleaseWait").attr("hidden", "true").toggle();
            $("#formReAnalyse").attr("hidden", "true");
        }

        // Show results for the reanalysed document
        function showReanalysedDoc(e) {
            // Change content with the text that was reanalysed
            var content = document.getElementById("content");
            var contentR = document.getElementById("contentR");
            content.innerHTML = contentR.innerHTML;

            // Change the tags with the new tags
            var divTags = document.getElementById("divTags");
            var divTagsR = document.getElementById("divTagsR");
            divTags.innerHTML = divTagsR.innerHTML;

            // Change the description with the new description
            var description = document.getElementById("description");
            var descriptionR = document.getElementById("descriptionR");
            description.innerHTML = descriptionR.innerHTML;

            // Change the AIDA form with the new form
            var sementisEntitiesAIDA = document.getElementById("sementisEntitiesAIDA");
            var sementisEntitiesAIDAR = document.getElementById("sementisEntitiesAIDAR");
            sementisEntitiesAIDA.innerHTML = sementisEntitiesAIDAR.innerHTML;

            // Remove the content of the result
            $("#resultReanalyse").empty();


            // Add colors to values and tags and show the details + other functions
            createArray();
            addColor();
            oneColor();
            colorAgain();
            addNamesForm();
            showDetails();
            hideDescriptionWhenSuggestions();
            showResultsDirectly();
            checkTextToReAnalyse();
            addTagOrValue();
            addToExistingTag();
        }

        function handleError(xhr, status) {
			alert('Error: ' + xhr.statusText);
		}


        // Update AIDA
        function showUpdate(e) {
            var update = $("#resultUpdate p").text();

            if (update === "success") {
				alert("Success! Thank you for updating!");
			}
            else {
				alert("Sorry, this functionality is still in progress! Try again later!");
			}
        }

