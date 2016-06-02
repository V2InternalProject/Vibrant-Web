function performanceHinderValidation() {
    $('#empCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#empCommentsFFSelf').val() == ' ' || $('#empCommentsFFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFSelf').val() == ' ' || $('#mngrCommentsFFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsFFSelfSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFSelfSecond').val() == ' ' || $('#mngrCommentsFFSelfSecond').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#reviewerCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#reviewerCommentsFFSelf').val() == ' ' || $('#reviewerCommentsFFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#empCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#empCommentsFFEnvi').val() == ' ' || $('#empCommentsFFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFEnvi').val() == ' ' || $('#mngrCommentsFFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsFFEnviSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsFFEnviSecond').val() == ' ' || $('#mngrCommentsFFEnviSecond').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#reviewerCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#reviewerCommentsFFEnvi').val() == ' ' || $('#reviewerCommentsFFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });

    $('#empCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#empCommentsIFSelf').val() == ' ' || $('#empCommentsIFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFSelf').val() == ' ' || $('#mngrCommentsIFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsIFSelfSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFSelfSecond').val() == ' ' || $('#mngrCommentsIFSelfSecond').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#reviewerCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#reviewerCommentsIFSelf').val() == ' ' || $('#reviewerCommentsIFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#hrCommentsIFSelf').rules("add", {
        required: function () {
            return ($('#hrCommentsIFSelf').val() == ' ' || $('#hrCommentsIFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#empCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#empCommentsIFEnvi').val() == ' ' || $('#empCommentsIFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsSupport').rules("add", {
        required: function () {
            return ($('#mngrCommentsSupport').val() == ' ' || $('#mngrCommentsSupport').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsSupportSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsSupportSecond').val() == ' ' || $('#mngrCommentsSupportSecond').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#reviewerCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#reviewerCommentsIFEnvi').val() == ' ' || $('#reviewerCommentsIFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#hrCommentsSupport').rules("add", {
        required: function () {
            return ($('#hrCommentsSupport').val() == ' ' || $('#hrCommentsSupport').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });

    $('#empCommentsSupport').rules("add", {
        required: function () {
            return ($('#empCommentsSupport').val() == ' ' || $('#empCommentsSupport').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFEnvi').val() == ' ' || $('#mngrCommentsIFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#mngrCommentsIFEnviSecond').rules("add", {
        required: function () {
            return ($('#mngrCommentsIFEnviSecond').val() == ' ' || $('#mngrCommentsIFEnviSecond').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });

    $('#reviewerCommentsSupport').rules("add", {
        required: function () {
            return ($('#reviewerCommentsSupport').val() == ' ' || $('#reviewerCommentsSupport').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#hrCommentsIFEnvi').rules("add", {
        required: function () {
            return ($('#hrCommentsIFEnvi').val() == ' ' || $('#hrCommentsIFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
    $('#hrCommentsFFSelf').rules("add", {
        required: function () {
            return ($('#hrCommentsFFSelf').val() == ' ' || $('#hrCommentsFFSelf').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });

    $('#hrCommentsFFEnvi').rules("add", {
        required: function () {
            return ($('#hrCommentsFFEnvi').val() == ' ' || $('#hrCommentsFFEnvi').val() == '');
        },
        messages:
			{
			    required: "Comment field is required."
			}
    });
}
function handleClick(approve) {
    $('#radioApproveValue').val(approve.value);
    if ($('#radioApproveValue').val() != 'Approved') {
        $("#mngrCommentsFFSelf").rules('remove', 'required');
        $("#mngrCommentsFFEnvi").rules('remove', 'required');
        $("#mngrCommentsIFSelf").rules('remove', 'required');
        $("#mngrCommentsSupport").rules('remove', 'required');
        $("#mngrCommentsIFEnvi").rules('remove', 'required');

        $("#mngrCommentsFFSelfSecond").rules('remove', 'required');
        $("#mngrCommentsFFEnviSecond").rules('remove', 'required');
        $("#mngrCommentsIFSelfSecond").rules('remove', 'required');
        $("#mngrCommentsSupportSecond").rules('remove', 'required');
        $("#mngrCommentsIFEnviSecond").rules('remove', 'required');

        $("#reviewerCommentsFFSelf").rules('remove', 'required');
        $("#reviewerCommentsFFEnvi").rules('remove', 'required');
        $("#reviewerCommentsIFSelf").rules('remove', 'required');
        $("#reviewerCommentsIFEnvi").rules('remove', 'required');
        $("#reviewerCommentsSupport").rules('remove', 'required');

        $("#hrCommentsFFSelf").rules('remove', 'required');
        $("#hrCommentsFFEnvi").rules('remove', 'required');
        $("#hrCommentsIFSelf").rules('remove', 'required');
        $("#hrCommentsIFEnvi").rules('remove', 'required');
        $("#hrCommentsSupport").rules('remove', 'required');
    }
}